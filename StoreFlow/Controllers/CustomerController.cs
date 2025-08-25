using StoreFlow.Models;
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

    public IActionResult GetCustomerListByCity()
    {
        var groupedCustomers = _context.Customers
            .ToList()
            .GroupBy(c => c.CustomerCity)
            .ToList();

        return View(groupedCustomers);
    }

    public IActionResult CustomersByCityCount()
    {
        var query = from c in _context.Customers
                    group c by c.CustomerCity into cityGroup
                    select new CustomerCityGroupViewModel
                    {
                        City = cityGroup.Key,
                        CustomerCount = cityGroup.Count()
                    };

        var result = query.OrderByDescending(c => c.CustomerCount).ToList();
        return View(result);
    }

    public IActionResult CustomerCityList()
    {
        var cities = _context.Customers
            .Select(c => c.CustomerCity)
            .Distinct()
            .ToList();

        return View(cities);
    }

    public IActionResult ParallelCustomers()
    {
        var customers = _context.Customers.ToList();
        var result = customers
            .AsParallel()
            .Where(c => c.CustomerCity
            // StringComparison kullanarak büyük/küçük harf duyarsız arama
            .StartsWith("A", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return View(result);
    }

    public IActionResult CustomerListExceptCityIstanbul()
    {
        // Except metodu ile İstanbul dışındaki müşterileri listeleme
        //var allCustomers = _context.Customers.ToList();
        //var istanbulCustomers = _context.Customers
        //    .Where(c => c.CustomerCity == "İstanbul")
        //    .ToList();
        //var result = allCustomers.Except(istanbulCustomers).ToList();

        // ExceptBy metodu ile İstanbul dışındaki müşterileri listeleme
        var istanbulCustomers = _context.Customers
            .Where(c => c.CustomerCity == "İstanbul")
            .Select(c => c.CustomerId) // Burada sadece ID'leri alır
            .ToList();
        var allCustomers = _context.Customers.ToList();
        var result = allCustomers.ExceptBy(istanbulCustomers, c => c.CustomerId).ToList();

        return View(result);
    }
}