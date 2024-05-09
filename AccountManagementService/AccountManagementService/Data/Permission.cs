namespace AccountManagementService.Data
{
    public enum Permission
    {
        SendTextMessages = 1,
        SendPhotos = 2,
        SendVideos = 3,
        SendFiles = 4,
        AddMembers = 10,
        DeleteMembers = 11,
        BanMembers = 12,
        ChangeChatInfo = 50,
        Administrator = 100,
    }
}