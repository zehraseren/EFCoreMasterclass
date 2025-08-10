using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _SalesDataComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _SalesDataComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var result = _context.Orders.OrderByDescending(x => x.OrderId).Include(y => y.Customers).Include(z => z.Products).Take(5).ToList();
        return View(result);
    }
}
