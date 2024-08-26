using Microsoft.AspNetCore.Mvc;

namespace WebSocketClientApp.Controllers
{
    public class WebSocketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}