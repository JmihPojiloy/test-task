namespace RESTApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ILogger<WebSocketController> _logger;

        public WebSocketController(
            WebSocketConnectionManager connectionManager,
            WebSocketHandler webSocketHandler,
            ILogger<WebSocketController> logger)
        {
            _connectionManager = connectionManager;
            _webSocketHandler = webSocketHandler;
            _logger = logger;
        }

        [HttpGet]
        public async Task GetMessagesWebSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _connectionManager.AddSocket(webSocket);
                _logger.LogInformation("WebSocket connection established");

                await _webSocketHandler.HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
