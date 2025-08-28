using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.RightSidebarComponents;

public class _MessageComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _MessageComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var messages = _context.Messages
            .Where(m => m.IsRead == false)
            .ToList();
        return View(messages);
    }
}