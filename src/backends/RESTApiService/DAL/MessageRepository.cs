using Npgsql;

namespace RESTApiService.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
            Refresh();
        }

        public async Task<IEnumerable<Message>> GetAllMessageAsync()
        {
            var messages = new List<Message>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT id, content, timestamp FROM messages", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var message = new Message
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    Timestamp = reader.GetDateTime(2)
                };

                messages.Add(message);
            }

            return messages;
        }

        public async Task<IEnumerable<Message>> GetMessagesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var messages = new List<Message>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(
                "SELECT id, content, timestamp FROM messages WHERE timestamp BETWEEN @startDate AND @endDate",
                connection
            );
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var message = new Message
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    Timestamp = reader.GetDateTime(2)
                };

                messages.Add(message);
            }

            return messages;
        }


        public async Task CreateMessageAsync(Message message)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("INSERT INTO messages (content, timestamp) VALUES (@content, @timestamp)", connection);
            command.Parameters.AddWithValue("@content", message.Content!);
            command.Parameters.AddWithValue("@timestamp", message.Timestamp);

            await command.ExecuteNonQueryAsync();
        }

        // вспомогательный метод для удобства тестирования сервиса
        private void Refresh()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var truncateCommand = new NpgsqlCommand("TRUNCATE TABLE messages RESTART IDENTITY", connection);
                truncateCommand.ExecuteNonQuery();

                var resetSequenceCommand = new NpgsqlCommand("ALTER SEQUENCE messages_id_seq RESTART WITH 1", connection);
                resetSequenceCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}