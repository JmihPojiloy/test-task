namespace RESTApiService.Services
{
    public class WebSocketConnectionManager
    {
        private readonly List<WebSocket> _webSockets = new();

        public void AddSocket(WebSocket socket)
        {
            _webSockets.Add(socket);
        }

        public async Task RemoveSocketAsync(WebSocket socket)
        {
            _webSockets.Remove(socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket Closed", CancellationToken.None);
        }

        public async Task SendMessageToAllAsync(Message message)
        {
            var newMessageJson = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(newMessageJson);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _webSockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}