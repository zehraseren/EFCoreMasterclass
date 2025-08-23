using StoreFlow.Enums;
using StoreFlow.Context;
using StoreFlow.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.Controllers;
public class TodoController : Controller
{
    private readonly StoreContext _context;

    public TodoController(StoreContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CreateTodo()
    {
        var todos = new List<Todo>
        {
            new Todo { Description = "Mail Gönder", Status = TaskStatusType.Pending, Priority = TodoPriorityType.Low },
            new Todo { Description = "Rapor Hazırla", Status = TaskStatusType.InProgress, Priority = TodoPriorityType.Medium },
            new Todo { Description = "Toplantı Yap", Status = TaskStatusType.Completed, Priority = TodoPriorityType.High }
        };

        await _context.Todos.AddRangeAsync(todos);
        await _context.SaveChangesAsync();
        return View();
    }

    public IActionResult TodoAggregatePriority()
    {
        var priorityFirstlyTodo = _context.Todos
            .Where(p => p.Priority == TodoPriorityType.High)
            .Select(d => d.Description)
            .ToList();

        string result = priorityFirstlyTodo.Aggregate(string.Empty, (acc, desc) => acc + $"<li>{desc}</li>");

        ViewBag.result = result;
        return View();
    }

    public IActionResult IncompletedTask()
    {
        var values = _context.Todos
            .Where(s => s.Status != TaskStatusType.Completed)
            .Select(d => d.Description)
            // AsEnumerable ile veritabanı sorgusu sonlandırılır ve LINQ to Objects işlemleri başlar
            // IEnumerable’e çevirip LINQ sorgusunu C# tarafında (in-memory) çalıştırılır
            // EF Core veritabanında desteklemeyen bir metot kullanmak istenilirse AsEnumerable() kullanılır
            .AsEnumerable()
            // Liste sonuna ekleme yapmak için Append kullanılır
            //.Append("Gün sonunda tüm görevleri kontrol etmeyi unutmayın!")
            // Liste başına ekleme yapmak için Prepend kullanılır
            .Prepend("Gün başında tüm görevleri kontrol etmeyi unutmayın!")
            // ToList ile sonuç listeye dönüştürülür yani IQueryable veya IEnumerable’i anında listeye dönüştürülür
            // Sonuçları dönüştürüp view’a gönderecekse veya daha sonra tekrar enumerate edilmeyecekse kullanılır
            // Burada tüm veri veritabanından çekilir ve belleğe alınır, LINQ işlemleri artık in-memory olur
            .ToList();

        return View(values);
    }

    public IActionResult TodoChunk()
    {
        var values = _context.Todos
            .Where(s => s.Status == TaskStatusType.Completed)
            .AsEnumerable()
            .Chunk(2)
            .ToList();

        return View(values);
    }

    public IActionResult TodoConcat()
    {
        var values = _context.Todos
            .Where(p => p.Priority == TodoPriorityType.High)
            .AsEnumerable()
            .Concat(_context.Todos.Where(p => p.Priority == TodoPriorityType.Medium))
            .ToList();

        return View(values);
    }

    public IActionResult TodoUnion()
    {
        var valuesHigh = _context.Todos
            .Where(p => p.Priority == TodoPriorityType.High)
            .ToList();

        var valuesMedium = _context.Todos
            .Where(p => p.Priority == TodoPriorityType.Medium)
            .ToList();

        var result = valuesHigh.UnionBy(valuesMedium, d => d.Description).ToList();

        return View(result);
    }
}
