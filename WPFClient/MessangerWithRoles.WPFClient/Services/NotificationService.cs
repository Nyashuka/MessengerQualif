using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.Services
{
    public class NotificationService : IService
    {
        private ClientWebSocket _webSocket;
        private CancellationTokenSource _cancellationToken;
        private EventBus _eventBus;
        private Task _notificationReceiviengTask;
        private Guid _clientGuid;
        public string ClientGuid => _clientGuid.ToString();

        public NotificationService()
        {
            _cancellationToken = new CancellationTokenSource();
            _eventBus = ServiceLocator.Instance.GetService<EventBus>();
            _clientGuid = Guid.NewGuid();
        }

        public void Start(string accessToken)
        {
            if (_cancellationToken != null)
                _cancellationToken.Cancel();

            if (_notificationReceiviengTask != null)
                _notificationReceiviengTask.Wait();

            _webSocket = new ClientWebSocket();
            _cancellationToken = new CancellationTokenSource();
            _notificationReceiviengTask = Task.Run(async () => await ConnectAndReceiveAsync(accessToken, _cancellationToken.Token));
        }

        public async Task ConnectAndReceiveAsync(string accessToken, CancellationToken cancellationToken)
        {
            try
            {
                _webSocket.Options.SetRequestHeader("accessToken", accessToken);
                _webSocket.Options.SetRequestHeader("clientGuid", _clientGuid.ToString());

                if (!(_webSocket.State == WebSocketState.Open))
                    await _webSocket.ConnectAsync(new Uri(APIEndpoints.NotificationsWS), CancellationToken.None);

                byte[] buffer = new byte[1024];

                while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    SocketResponse? response = JsonSerializer.Deserialize<SocketResponse>(receivedData);

                    if (response != null)
                    {
                        NotifyClient_MessageReceived(response);
                    }
                }

                if (_webSocket.State == WebSocketState.Open && cancellationToken.IsCancellationRequested)
                {
                    await StopReceiving();
                    MessageBox.Show("Disconnected from notification server.");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The websocket has already been started") || _webSocket.State == WebSocketState.Open)
                {
                    await StopReceiving();
                    MessageBox.Show("Disconnected from notification server.");
                }

                //at System.Net.WebSockets.ClientWebSocketOptions.ThrowIfReadOnly() at System.Net.WebSockets.ClientWebSocketOptions.SetRequestHeader(String headerName, String headerValue) at MessengerWithRoles.WPFClient.Services.NotificationService.d__6.MoveNext() in E:\QualifWork\WPFClient\MessangerWithRoles.WPFClient\Services\NotificationService.cs:line 49
                if (!cancellationToken.IsCancellationRequested)
                    MessageBox.Show("Notification service error!\n" + e.Message);

                if(!e.Message.Contains("The operation was canceled"))
                    MessageBox.Show("Restart application to reconnect notification service");
            }
            finally
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed.", CancellationToken.None);
                    MessageBox.Show("Disconnected from notification server.");
                }

                
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

        public async Task StopReceiving()
        {
            _cancellationToken.Cancel();
            if (_notificationReceiviengTask != null)
                _notificationReceiviengTask.Wait();
            try
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed.", CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while closing the WebSocket connection: " + ex.Message);
            }
        }
    }
}
