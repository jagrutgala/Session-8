using Microsoft.AspNetCore.DataProtection;

namespace UserApp.Api.Models

{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
