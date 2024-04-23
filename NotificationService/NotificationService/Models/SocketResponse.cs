﻿using NotificationService.Data;

namespace NotificationService.Models
{
    public class SocketResponse
    {
        public SocketResponseType ResponseType { get; set; }
        public string? JsonData { get; set; }
    }
}
