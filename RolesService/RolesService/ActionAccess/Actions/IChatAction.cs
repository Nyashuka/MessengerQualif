﻿namespace RolesService.ActionAccess.Actions
{
    public interface IChatAction
    {
        Task<bool> HasAccess(HttpClient httpClient);
    }
}