using Microsoft.AspNetCore.DataProtection;

namespace UserApp.Api.Models
{
    public class UserUpdateRequestModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
