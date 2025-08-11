using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.StatisticViewComponents;

public class _WidgetComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _WidgetComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.categoryCount = _context.Categories.Count();

        ViewBag.productMaxPrice = _context.Products.Max(x => x.ProductPrice);

        ViewBag.productMaxPriceProductName = _context.Products.Where(x => x.ProductPrice == (_context.Products.Max(y => y.ProductPrice))).Select(z => z.ProductName).FirstOrDefault();

        ViewBag.productMinPrice = _context.Products.Min(x => x.ProductPrice);

        ViewBag.productMinPriceProductName = _context.Products.Where(x => x.ProductPrice == (_context.Products.Min(y => y.ProductPrice))).Select(z => z.ProductName).FirstOrDefault();

        ViewBag.totalSumProductStock = _context.Products.Sum(x => x.ProductStock);

        ViewBag.avgProductStock = _context.Products.Average(x => x.ProductStock);

        ViewBag.avgProductPrice = _context.Products.Average(x => x.ProductPrice);

        ViewBag.biggerPriceThan1000ProductCount = _context.Products.Where(x => x.ProductPrice > 1000).Count();

        ViewBag.stockCountBiggerThan50AndSmaller100ProductCount = _context.Products.Where(x => x.ProductStock > 50 && x.ProductStock < 100).Count();

        return View();
    }
}
