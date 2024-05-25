using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace NotificationService.Services
{
    public class ConnectionsService : IConnectionsService
    {
        public Dictionary<int, Dictionary<Guid, WebSocket>> Clients { get; set; } = new();

        public async Task AddClientAndStartReceiving(int userId, Guid clientGuid, WebSocket client)
        {
            if (!Clients.ContainsKey(userId))
            {
                Clients[userId] = new Dictionary<Guid, WebSocket>();
            }

            Clients[userId][clientGuid] = client;

            await HandleWebSocket(userId, clientGuid, client);
        }

        public async Task RemoveClientAndCloseAsync(int userId, Guid clientGuid)
        {
            if (Clients.TryGetValue(userId, out var clients))
            {
                if (clients.Count > 0)
                {
                    var clientToRemove = Clients[userId][clientGuid];
                    await clientToRemove.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                          "Closing WebSocket connection",
                                          CancellationToken.None);
                    clients.Remove(clientGuid);
                }
            }
        }

        public async Task SendMessageForUserAsync(string message, int userId)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            Clients.TryGetValue(userId, out var webSockets);

            if (webSockets == null)
                return;

            foreach (var item in webSockets)
            {
                await item.Value.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task HandleWebSocket(int userId, Guid clientGuid, WebSocket webSocket)
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
                    await RemoveClientAndCloseAsync(userId, clientGuid);
                }
            }
        }

        public List<WebSocket> GetActiveConnections(List<int> dataUsers, int senderId, Guid clientGuid)
        {
            List<WebSocket> activeConnections = new List<WebSocket>();

            foreach (int userId in dataUsers)
            {
                if (Clients.TryGetValue(userId, out var sockets))
                {
                    foreach (var socket in sockets)
                    {
                        if (socket.Value.State == WebSocketState.Open)
                        {
                            activeConnections.Add(socket.Value);
                        }
                    }

                }
            }

            if(Clients.TryGetValue(senderId, out var senderSockets))
            {
                if(senderSockets.Count > 1)
                {
                    foreach (var socket in senderSockets)
                    {
                        if (socket.Key != clientGuid && socket.Value.State == WebSocketState.Open)
                        {
                            activeConnections.Add(socket.Value);
                        }
                    }
                }
            }

            return activeConnections;
        }
    }
}
