using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
