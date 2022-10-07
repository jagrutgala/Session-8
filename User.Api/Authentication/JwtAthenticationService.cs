using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UserApp.Api.Authentication
{
    public class JwtAthenticationService : IJwtAthenticationService
    {

        readonly IConfiguration _configuration;

        public JwtAthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] GetSecretKey()
        {
            string key = _configuration["Jwt:ApplicationSecretKey"];
            byte[] secretKey = Encoding.UTF8.GetBytes(key); // Get SecretKey in Bytes
            return secretKey;
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),

                ValidateAudience = true,
                ValidAudience = GetAudience(),

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymetricKey(),

                //ValidateLifetime = true,
            };
            return validationParameters;
        }

        public JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            JwtSecurityToken jwtToken = new JwtSecurityToken(
               issuer: GetIssuer(),
               audience: GetAudience(),
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(10),
               signingCredentials: GetSignedCredentials(SecurityAlgorithms.HmacSha256)
            );
            return jwtToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes( randomNumber );
            return Convert.ToBase64String( randomNumber );
        }

        private SymmetricSecurityKey GetSymetricKey()
        {
            byte[] secretKey = GetSecretKey();
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey( secretKey ); // Generate Symetrics key for our secret key
            return symmetricKey;
        }

        private SigningCredentials GetSignedCredentials( string algorithm )
        {
            SymmetricSecurityKey symmetricKey = GetSymetricKey();
            SigningCredentials signedKey = new SigningCredentials( symmetricKey, algorithm );
            return signedKey;
        }

        private string GetIssuer()
        {
            string issuer = _configuration["Jwt:Issuer"] ?? "Application";
            return issuer;
        }

        private string GetAudience()
        {
            string audience = _configuration["Jwt:Audience"] ?? "Application";
            return audience;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken( string? token )
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymetricKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken( token, tokenValidationParameters, out SecurityToken securityToken );
            if ( securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals( SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase ) )
                throw new SecurityTokenException( "Invalid token" );

            return principal;
        }
    }
}
