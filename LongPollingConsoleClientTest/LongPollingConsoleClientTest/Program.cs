using System.Net.Http.Json;

namespace LongPollingConsoleClientTest
{
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string serverUrl = "http://127.0.0.1:6999/api/chat";

            Console.Write("Your name: ");
            string user = Console.ReadLine();

            _ = PollMessages(client, serverUrl, user);

            while (true)
            {
                Console.Write("Enter message: ");
                string message = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(message))
                {
                    await SendMessage(client, serverUrl, user, message);
                }
            }
        }

        static async Task PollMessages(HttpClient client, string serverUrl, string user)
        {
            while (true)
            {
                try
                {
                    var messages = await client.GetFromJsonAsync<List<ChatMessage>>($"{serverUrl}/poll?user={user}");

                    if (messages != null && messages.Count > 0)
                    {
                        // Вивід отриманих повідомлень
                        foreach (var message in messages)
                        {
                            Console.WriteLine($"[{message.Timestamp}] {message.User}: {message.Message}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                    // Ви можете додати затримку або логіку для обробки помилок
                }
            }
        }

        static async Task SendMessage(HttpClient client, string serverUrl, string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync($"{serverUrl}/send", chatMessage);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Помилка при відправленні повідомлення: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Помилка: {e.Message}");
            }
        }
    }
}
