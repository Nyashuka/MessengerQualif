// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace DatabaseService.Models.DatabaseModels
{
    public class AccessToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
