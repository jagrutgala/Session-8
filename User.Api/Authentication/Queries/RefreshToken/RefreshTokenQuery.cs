using MediatR;
using UserApp.Api.Authentication.Common;

namespace UserApp.Api.Authentication.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<UserAuthenticationResponse>
    {
        public RefreshTokenQuery(
            UserAuthenticationResponse authenticationTokens
        )
        {
            AuthenticationTokens = authenticationTokens;
        }

        public UserAuthenticationResponse AuthenticationTokens { get; }
    }
}
