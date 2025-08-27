using StoreFlow.Models;
using StoreFlow.Context;
using StoreFlow.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreFlow.Controllers;
public class ProductController : Controller
{
    private readonly StoreContext _context;

    public ProductController(StoreContext context)
    {
        _context = context;
    }

    public IActionResult ProductList()
    {
        var products = GetProductsWithCategory();
        return View(products);
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        LoadCategories();
        return View();
    }

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return RedirectToAction("ProductList");
    }

    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        _context.Remove(product);
        _context.SaveChanges();
        return RedirectToAction("ProductList");
    }

    [HttpGet]
    public IActionResult UpdateProduct(int id)
    {
        LoadCategories();
        var product = _context.Products.Find(id);
        return View(product);
    }

    [HttpPost]
    public IActionResult UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
        return RedirectToAction("ProductList");
    }

    public IActionResult ListProductsByCategory(int id)
    {
        var products = GetProductsWithCategory()
            .Where(p => p.CategoryId == id)
            .ToList();

        if (!products.Any()) return NotFound();

        ViewBag.categoryName = products.First().CategoryName;

        return View(products);
    }

    private void LoadCategories()
    {
        ViewBag.categories = _context.Categories
            .Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            })
            .OrderBy(x => x.Text)
            .ToList();
    }

    private List<ProductListByCategoryViewModel> GetProductsWithCategory()
    {
        var product = _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductListByCategoryViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductStock = p.ProductStock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,
            }).ToList();

        return product;
    }

    public IActionResult First5ProductsList()
    {
        var products = GetProductsWithCategory().Take(5).ToList();
        return View(products);
    }

    public IActionResult Skip4ProductsList()
    {
        var products = GetProductsWithCategory().Skip(4).Take(10).ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult CreateProductWithAttach()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateProductWithAttach(Product product)
    {
        var category = new Category { CategoryId = 1 };
        _context.Categories.Attach(category);

        var productValue = new Product
        {
            ProductName = product.ProductName,
            ProductPrice = product.ProductPrice,
            ProductStock = product.ProductStock,
            Category = category
        };
        _context.Products.Add(productValue);
        _context.SaveChanges();

        return RedirectToAction("ProductList");
    }

    public IActionResult ProductCount()
    {
        var value = _context.Products.LongCount();
        ViewBag.count = value;

        var lastProduct = _context.Products.OrderBy(p => p.ProductId).Last();
        ViewBag.lastProduct = lastProduct.ProductName;
        return View();
    }
}
