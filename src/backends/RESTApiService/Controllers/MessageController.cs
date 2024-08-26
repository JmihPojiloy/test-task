namespace RESTApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _repository;
        private readonly ILogger<MessageController> _logger;
        private readonly WebSocketConnectionManager _connectionManager;

        public MessageController(
            IMessageRepository repository,
            ILogger<MessageController> logger,
            WebSocketConnectionManager connectionManager)
        {
            _repository = repository;
            _logger = logger;
            _connectionManager = connectionManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Message message)
        {
            message.Timestamp = DateTime.Now;
            await _repository.CreateMessageAsync(message);
            _logger.LogInformation("Created a new message");

            // Уведомляем всех WebSocket-клиентов
            await _connectionManager.SendMessageToAllAsync(message);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesByDateRange()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddMinutes(-10);
            var messages = await _repository.GetMessagesByDateRangeAsync(startDate, endDate);
            return Ok(messages);
        }
    }
}



