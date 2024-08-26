namespace RESTApiService.DAL
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<Message>> GetAllMessageAsync();
        Task<IEnumerable<Message>> GetMessagesByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task CreateMessageAsync(Message message);
    }
}