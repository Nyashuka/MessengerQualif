﻿
namespace NotificationService.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

