using Microsoft.AspNetCore.Identity;

namespace UserApp.Infrastructure.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public DateTime DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
