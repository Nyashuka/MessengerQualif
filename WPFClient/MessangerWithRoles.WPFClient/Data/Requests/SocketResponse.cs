using MessengerWithRoles.WPFClient.Data;

namespace MessengerWithRoles.WPFClient.Data.Requests
{
    public class SocketResponse
    {
        public SocketResponseType ResponseType { get; set; }
        public string? JsonData { get; set; }
    }
}
