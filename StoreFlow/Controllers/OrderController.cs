using StoreFlow.Enums;
using StoreFlow.Context;
using StoreFlow.Helpers;
using StoreFlow.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreFlow.Controllers;
public class OrderController : Controller
{
    private readonly StoreContext _context;

    public OrderController(StoreContext context)
    {
        _context = context;
    }

    public IActionResult OrderList()
    {
        var orders = _context.Orders.ToList();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrder(int id)
    {
        await LoadProducts();
        await LoadCustomers();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        order.Status = OrderStatusType.OrderReceived;
        order.OrderDate = DateTime.Now;
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("OrderList");
    }

    // GET: Ajax ile ürünün fiyatını çekmek için
    [HttpGet]
    public async Task<JsonResult> GetProductPrice(int productId)
    {
        // Gelen productId parametresine göre ilgili ürünü DB'den async olarak çekilir
        var price = await _context.Products         // ProductId eşleşen ürünü filtrele
            .Where(p => p.ProductId == productId)   // Sadece ProductPrice alanını seçilir
            .Select(p => p.ProductPrice)            // Eğer ürün varsa fiyatı alır, yoksa default (0) döner
            .FirstOrDefaultAsync();

        // Fiyat bilgisini JSON formatında frontend'e geri gönderir
        return Json(price);
    }

    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("OrderList");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateOrder(int id)
    {
        await LoadProducts();
        await LoadCustomers();

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

        ViewBag.statusList = LoadOrderStatusType(order.Status);

        if (order.ProductId != 0)
        {
            var productPrice = await _context.Products
                .Where(p => p.ProductId == order.ProductId)
                .Select(p => p.ProductPrice)
                .FirstOrDefaultAsync();

            ViewBag.unitPrice = productPrice;
        }

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("OrderList");
    }

    public IActionResult AllStockSmallerThan5()
    {
        bool orderStockCount = _context.Orders.All(o => o.OrderCount <= 5);

        if (orderStockCount)
        {
            ViewBag.message = "Tüm siparişler 5 adetten küçüktür.";
        }
        else
        {
            ViewBag.message = "Tüm siparişler 5 adetten büyüktür.";
        }
        return View();
    }

    public IActionResult OrderListByStatus(OrderStatusType? status)
    {
        ViewBag.statusList = LoadOrderStatusType(status);

        IQueryable<Order> query = _context.Orders
            .Include(c => c.Customers)
            .Include(p => p.Products);

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var result = query.ToList();

        if (!result.Any())
        {
            var statusText = status.HasValue ? status.Value.GetDescription() : "Tüm Siparişler";
            ViewBag.message = $"{statusText} için sipariş bulunmamaktadır.";
        }

        return View(result);
    }

    public IActionResult OrderListSearch(string name, string filterType)
    {
        var orders = _context.Orders.ToList();

        if (filterType == "start")
        {
            // DB'den alır
            var list = _context.Orders.AsEnumerable();

            // Description'a göre filtreleme yapar
            list = list.Where(o => o.Status.ToString().GetDescription().StartsWith(name, StringComparison.OrdinalIgnoreCase));

            return View(list.ToList());
        }
        else if (filterType == "end")
        {
            var values = orders
             .Where(o => o.Status.ToString().GetDescription()
                 .EndsWith(name, StringComparison.OrdinalIgnoreCase))
             .ToList();

            return View(values);
        }

        return View(orders);
    }

    public async Task<IActionResult> OrderListAsynchronous()
    {
        var values = await _context.Orders
            .Include(p => p.Products)
            .Include(c => c.Customers)
            .ToListAsync();

        return View(values);
    }

    // Müşteri dropdown list
    private async Task LoadCustomers()
    {
        ViewBag.customers = await _context.Customers
            .Select(c => new SelectListItem
            {
                Text = c.CustomerName + " " + c.CustomerSurname,
                Value = c.CustomerId.ToString(),
            })
            .OrderBy(x => x.Text)
            .ToListAsync();
    }

    // Ürün dropdown list
    private async Task LoadProducts()
    {
        ViewBag.products = await _context.Products
            .Select(p => new SelectListItem
            {
                Text = p.ProductName,
                Value = p.ProductId.ToString(),
            }).
            OrderBy(p => p.Text)
            .ToListAsync();
    }

    // Enum'dan Status dropdown listesi
    // Bu metotta sadece enum değerleri bellekte dönüyor, DB ve harici I/O (input/output) yok dolayısıyla async olmasına gerek yok
    private List<SelectListItem> LoadOrderStatusType(OrderStatusType? selected = null)
    {
        var list = Enum.GetValues(typeof(OrderStatusType))
            .Cast<OrderStatusType>()
            .Select(o => new SelectListItem
            {
                Text = o.GetDescription(),
                Value = ((int)o).ToString(),
                Selected = selected.HasValue && selected.Value == o
            })
            .ToList();

        // İlk seçenek, seçilemez
        list.Insert(0, new SelectListItem
        {
            Text = "Sipariş Durumu",
            Value = "",
            Selected = !selected.HasValue,
            Disabled = true
        });

        return list;
    }
}
