using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _ActivityComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _ActivityComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var result = _context.Activities.ToList();
        return View(result);
    }
}
