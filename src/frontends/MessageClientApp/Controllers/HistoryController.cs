using MessageClientApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketClientApp.Controllers
{
    public class HistoryController : Controller
    {
        private readonly HttpApiService _httpApiService;

        public HistoryController(HttpApiService httpApiService)
        {
            _httpApiService = httpApiService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _httpApiService.GetByTenMinutes();
            return View(messages);
        }
    }
}