namespace UserApp.Api.Authentication.Common
{
    public class UserAuthenticationResponse
    {
        public UserAuthenticationResponse(
            string token,
            string refreshToken,
            DateTime expiration
        )
        {
            Token = token;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
