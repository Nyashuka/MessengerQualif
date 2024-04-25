using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.Services
{
    public class MessagesService : IService
    {
        private ClientWebSocket _webSocket;
        private const string ConnectUri = "ws://127.0.0.1:6999/api/Notification/connect";
        private readonly CancellationTokenSource _cancellationToken;
        private EventBus _eventBus;
        private Task _thread;

        public MessagesService()
        {
            _cancellationToken = new CancellationTokenSource();
            _eventBus = ServiceLocator.Instance.GetService<EventBus>();
        }

        public void Start(string accessToken)
        {
            _thread = Task.Run(async () => await ConnectAndReceiveAsync(accessToken));
        }

        public async Task ConnectAndReceiveAsync(string accessToken)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _webSocket = new ClientWebSocket();
                    _webSocket.Options.SetRequestHeader("accessToken", accessToken);

                    await _webSocket.ConnectAsync(new Uri(ConnectUri), CancellationToken.None);

                    byte[] buffer = new byte[1024];

                    while (_webSocket.State == WebSocketState.Open || !_cancellationToken.IsCancellationRequested)
                    {
                        WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        SocketResponse? response = JsonSerializer.Deserialize<SocketResponse>(receivedData);

                        if (response != null)
                        {
                            NotifyClient_MessageReceived(response);
                            //MessageBox.Show($"Received data from server: {response.JsonData}");
                        }

                    }

                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed.", CancellationToken.None);
                    MessageBox.Show("Disconnected from notification server.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Notification service error!(reconnect after 5 seconds)\n" + e.Message);
                }

                await Task.Delay(5000);
            }
        }

        private void NotifyClient_MessageReceived(SocketResponse response)
        {
            if (response.ResponseType == SocketResponseType.TextMessage)
            {
                MessageDto message = JsonSerializer.Deserialize<MessageDto>(response.JsonData);

                ServiceLocator.Instance.GetService<EventBus>().Raise(EventBusDefinitions.TextMessageReceived, 
                    new TextMessageEventBusArgs(message));
            }
        }

        public void StopReceiving()
        {
            _cancellationToken.Cancel(false);
        }
    }
}
