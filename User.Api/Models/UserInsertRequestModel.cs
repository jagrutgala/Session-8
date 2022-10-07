using Microsoft.AspNetCore.DataProtection;

namespace UserApp.Api.Models
{
    public class UserInsertRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
