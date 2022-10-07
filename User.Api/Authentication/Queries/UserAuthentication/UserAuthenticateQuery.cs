using MediatR;
using UserApp.Api.Authentication.Common;

namespace UserApp.Api.Authentication.Queries.UserAuthentication
{
    public class UserAuthenticateQuery : IRequest<UserAuthenticationResponse>
    {
        public UserAuthenticateQuery(
            UserAuthenticationRequest authenticationRequest
        )
        {
            AuthenticationRequest = authenticationRequest;
        }

        public UserAuthenticationRequest AuthenticationRequest { get; }
    }
}
