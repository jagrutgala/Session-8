using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserApp.Api.Authentication
{
    public interface IJwtAthenticationService
    {
        public byte[] GetSecretKey();
        public TokenValidationParameters GetTokenValidationParameters();
        public JwtSecurityToken GenerateToken( IEnumerable<Claim> claims );
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetPrincipalFromExpiredToken( string? token );

    }
}