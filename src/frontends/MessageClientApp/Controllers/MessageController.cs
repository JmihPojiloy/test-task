using MessageClientApp.Services;
using Microsoft.AspNetCore.Mvc;
using shared.Models;

namespace MessageClientApp.Controllers
{
    public class MessageController : Controller
    {

        private readonly HttpApiService _httpApiService;

        public MessageController(HttpApiService httpApiService)
        {
            _httpApiService = httpApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Message message)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), message);
            }

            await _httpApiService.Create(message);
            return RedirectToAction(nameof(Index));
        }
    }
}