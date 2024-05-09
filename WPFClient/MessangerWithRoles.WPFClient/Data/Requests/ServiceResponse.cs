namespace MessengerWithRoles.WPFClient.Data.Requests
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public ServiceResponse()
        {

        }

        public ServiceResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public ServiceResponse(T? data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

    }
}