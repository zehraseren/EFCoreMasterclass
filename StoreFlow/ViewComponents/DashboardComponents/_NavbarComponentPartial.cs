using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _NavbarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}

