using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _CardStatisticsComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _CardStatisticsComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.totalCustomerCount = _context.Customers.Count();
        ViewBag.totalCategoryCount = _context.Categories.Count();
        ViewBag.totalProductCount = _context.Products.Count();
        ViewBag.avgCustomerBalance = _context.Customers.Average(x=>x.CustomerBalance);
        ViewBag.totalOrderCount = _context.Orders.Count();
        ViewBag.sumOrderProductCount = _context.Orders.Sum(x => x.OrderCount);
        return View();
    }
}
