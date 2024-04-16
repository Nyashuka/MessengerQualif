using System.Net.WebSockets;
using System.Text;

namespace WebSocketConsoleClientTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string uri = "ws://127.0.0.1:6999/api/Notification/connect";

            string? name = string.Empty;
            //name = Console.ReadLine();
            name = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InRlc3QiLCJleHAiOjE3MTU0NjYxMTR9.vB6fnfE4Cw-BQrUEpooWPPIqay8ll_FoFhe4GbDyKXviN8UQubxj8LQrzFId8Yii1Pss24Z3gaNbXAo91qeksg";

            using (ClientWebSocket socket = new ClientWebSocket())
            {
                socket.Options.SetRequestHeader("accessToken", name);

                await socket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await Console.Out.WriteLineAsync("Connected!");

                byte[] buffer = new byte[1024];

                while (socket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    Console.WriteLine($"Received data from server: {receivedData}");
                }

                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed.", CancellationToken.None);
                Console.WriteLine("Disconnected from server.");
            }
        }
    }
}
