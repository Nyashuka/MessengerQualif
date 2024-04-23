﻿using MessagesService.DTOs;

namespace MessagesService.Services.Interfaces
{
    public interface IMessageService
    {
        Task HandleMessage(int senderId, string accessToken, ClientMessageDTO clientMessageDTO);
    }
}
