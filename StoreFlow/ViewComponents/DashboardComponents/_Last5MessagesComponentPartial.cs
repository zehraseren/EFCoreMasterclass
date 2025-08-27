using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _Last5MessagesComponentPartial : ViewComponent
{
    private readonly StoreContext _context;

    public _Last5MessagesComponentPartial(StoreContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var messages = _context.Messages
            .OrderBy(m => m.MessageId)
            .ToList()
            .TakeLast(5)
            .AsEnumerable();
        return View(messages);
    }
}
