using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace NotificationService.Services
{
    public class ConnectionsService : IConnectionsService
    {
        public Dictionary<int, WebSocket> Clients { get; set; } = new();

        public async Task AddClientAndStartReceiving(int userId, WebSocket client)
        {
            if (!Clients.TryAdd(userId, client))
                throw new Exception("Cant add client with user id=" + userId);

           await HandleWebSocket(userId, client);
        }

        public async Task RemoveClientAndCloseAsync(int userId)
        {
            Clients.Remove(userId, out var removedClient);

            if (removedClient == null)
                throw new Exception("Client with user id=" + userId + " does not exists!");


            await removedClient.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                           "Closing WebSocket connection",
                                           CancellationToken.None);
        }

        public async Task SendMessageForUserAsync(string message, int userId)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            Clients.TryGetValue(userId, out var webSocket);

            if (webSocket == null)
                return;

            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task HandleWebSocket(int userId, WebSocket webSocket)
        {
            var buffer = new byte[1024];
            var segment = new ArraySegment<byte>(buffer);

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                    if (result.CloseStatus.HasValue)
                    {
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                    else
                    {
                        string receivedMessage = Encoding.UTF8.GetString(segment.Array, 0, result.Count);

                        var responseMessage = "Received!";
                        var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                        var responseSegment = new ArraySegment<byte>(responseBuffer);
                        await webSocket.SendAsync(responseSegment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {userId}: {ex.Message}");
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing WebSocket connection", CancellationToken.None);
                }

                if (Clients.ContainsKey(userId))
                {
                    await RemoveClientAndCloseAsync(userId);
                }
            }
        }

        public List<WebSocket> GetActiveConnections(List<int> dataUsers)
        {
            List<WebSocket> activeConnections = new List<WebSocket>();

            foreach (int userId in dataUsers)
            {
                if (Clients.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
                {
                    activeConnections.Add(socket);
                }
            }

            return activeConnections;
        }
    }
}
