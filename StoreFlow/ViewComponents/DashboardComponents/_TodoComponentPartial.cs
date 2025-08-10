using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _TodoComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _TodoComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var result = _context.Todos.OrderByDescending(x => x.TodoId).Take(6).ToList();
        return View(result);
    }
}
