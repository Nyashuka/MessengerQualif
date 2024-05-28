namespace DatabaseService.Data
{
    public enum PermissionEnum
    {
        SendTextMessages = 1,
        SendPhotos = 2,
        SendVideos = 3,
        SendFiles = 4,
        DeleteMessages = 9,
        AddMembers = 10,
        DeleteMembers = 11,
        BanMembers = 12,
        CreateRoles = 13,
        GiveRoles = 14,
        EditRoles = 15,
        DeleteRoles = 16,
        ChangeChatInfo = 50,
        Administrator = 100,
    }
}
