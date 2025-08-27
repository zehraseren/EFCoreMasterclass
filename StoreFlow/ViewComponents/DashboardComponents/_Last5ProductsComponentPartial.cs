using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _Last5ProductsComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _Last5ProductsComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var products = _context.Products
            .OrderBy(p => p.ProductId)
            .AsEnumerable() // EF sorgusunu SQL'den getir, sonra LINQ-to-Objects
            .SkipLast(5)
            .TakeLast(6)
            .ToList();
        return View(products);
    }
}
