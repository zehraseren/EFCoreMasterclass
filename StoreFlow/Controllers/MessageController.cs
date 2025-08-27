using StoreFlow.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StoreFlow.Controllers;
public class MessageController : Controller
{
    private readonly StoreContext _context;

    public MessageController(StoreContext context)
    {
        _context = context;
    }

    public IActionResult MessageList()
    {
        var messages = _context.Messages.AsNoTracking().ToList();
        return View(messages);
    }

    [HttpPost]
    public IActionResult MarkAsRead(int id)
    {
        var message = _context.Messages.FirstOrDefault(m => m.MessageId == id);
        if (message == null)
            return NotFound();

        message.IsRead = true;
        _context.Update(message);
        _context.SaveChanges();

        return Json(new { success = true });
    }
}
