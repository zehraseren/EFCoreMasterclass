using StoreFlow.Context;
using StoreFlow.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.Controllers;
public class CategoryController : Controller
{
    private readonly StoreContext _context;

    public CategoryController(StoreContext context)
    {
        _context = context;
    }

    public IActionResult CategoryList()
    {
        var result = _context.Categories.ToList();
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        category.CategoryStatus = false;
        _context.Categories.Add(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }

    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        _context.Remove(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }

    [HttpGet]
    public IActionResult UpdateCategory(int id)
    {
        var category = _context.Categories.Find(id);
        return View(category);
    }

    [HttpPost]
    public IActionResult UpdateCategory(Category category)
    {
        category.CategoryStatus = !category.CategoryStatus;
        _context.Categories.Update(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }

    public IActionResult ChangeCategoryStatus(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null) return NotFound();
        category.CategoryStatus = !category.CategoryStatus;
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }
}
