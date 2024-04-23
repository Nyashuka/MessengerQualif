using MessengerWithRoles.WPFClient.Data;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class SocketResponse
    {
        public SocketResponseType ResponseType { get; set; }
        public string? JsonData { get; set; }
    }
}
