namespace RESTApiService.Services
{
    public class WebSocketHandler
    {
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly ILogger<WebSocketHandler> _logger;

        public WebSocketHandler(WebSocketConnectionManager connectionManager, ILogger<WebSocketHandler> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var segment = new ArraySegment<byte>(buffer);

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Обработка входящих сообщений от клиента
                    var message = receivedMessage;
                    var response = JsonSerializer.Serialize(message);
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(response)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _connectionManager.RemoveSocketAsync(webSocket);
                    _logger.LogInformation("WebSocket connection closed.");
                }
            }
        }
    }
}