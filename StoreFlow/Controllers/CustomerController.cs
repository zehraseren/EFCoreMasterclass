using StoreFlow.Context;
using StoreFlow.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.Controllers;
public class CustomerController : Controller
{
    private readonly StoreContext _context;

    public CustomerController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult CreateCustomer()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return RedirectToAction("CustomerList");
    }

    public IActionResult DeleteCustomer(int id)
    {
        var customer = _context.Customers.Find(id);
        _context.Remove(customer);
        _context.SaveChanges();
        return RedirectToAction("CustomerList");
    }

    [HttpGet]
    public IActionResult UpdateCustomer(int id)
    {
        var Customer = _context.Customers.Find(id);
        return View(Customer);
    }

    [HttpPost]
    public IActionResult UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
        return RedirectToAction("CustomerList");
    }

    public IActionResult CustomerListOrderByCustomerName()
    {
        var customers = _context.Customers.OrderBy(c => c.CustomerName + c.CustomerSurname).ToList();
        return View(customers);
    }

    public IActionResult CustomerListOrderByDescBalance()
    {
        var customers = _context.Customers.OrderByDescending(c => c.CustomerBalance).ToList();
        return View(customers);
    }

    public IActionResult CustomerListByCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ViewBag.message = null;
            return View();
        }

        var exist = _context.Customers.Any(c => c.CustomerCity == city);
        var count = _context.Customers.Count(c => c.CustomerCity == city);

        if (exist)
            ViewBag.message = $"{city} şehrinde en az 1 müşteri var.Kişi Sayısı: {count}";
        else
            ViewBag.message = $"{city} şehrinde hiç müşteri yok";

        return View();
    }
}
