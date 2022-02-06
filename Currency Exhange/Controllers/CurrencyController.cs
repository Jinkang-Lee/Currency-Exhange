using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
