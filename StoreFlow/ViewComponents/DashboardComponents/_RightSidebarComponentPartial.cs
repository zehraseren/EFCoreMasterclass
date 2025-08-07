using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents.DashboardComponents;

public class _RightSidebarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
