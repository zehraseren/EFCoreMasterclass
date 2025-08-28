using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.RightSidebarComponents;

public class _TodoListComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _TodoListComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var result = _context.Todos
            .OrderBy(t => t.TodoId)
            .AsEnumerable()
            .TakeLast(10)
            .ToList();
        return View(result);
    }
}
