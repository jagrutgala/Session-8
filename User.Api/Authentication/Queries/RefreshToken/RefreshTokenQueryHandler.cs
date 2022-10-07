using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApp.Api.Authentication.Common;
using UserApp.Infrastructure.Entities;
using static Humanizer.In;

namespace UserApp.Api.Authentication.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, UserAuthenticationResponse>
    {
        private readonly IJwtAthenticationService _jwtAthenticationService;
        private readonly UserManager<User> _userManager;

        public RefreshTokenQueryHandler(
            IJwtAthenticationService jwtAthenticationService,
            UserManager<User> userManager


        )
        {
            this._jwtAthenticationService = jwtAthenticationService;
            this._userManager = userManager;
        }
        async Task<UserAuthenticationResponse> IRequestHandler<RefreshTokenQuery, UserAuthenticationResponse>.Handle( RefreshTokenQuery request, CancellationToken cancellationToken )
        {
            if ( request.AuthenticationTokens is null )
            {
                throw new ApplicationException( "Invalid tokens" );
            }
            string accessToken = request.AuthenticationTokens.Token;
            string refreshToken = request.AuthenticationTokens.RefreshToken;

            var principal = _jwtAthenticationService.GetPrincipalFromExpiredToken( accessToken );
            if ( principal is null )
            {
                throw new ApplicationException( "Invalid tokens" );
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync( username );

            if ( user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now )
            {
                throw new ApplicationException( "Invalid tokens" );
            }

            var newAccessToken = _jwtAthenticationService.GenerateToken( principal.Claims.ToList() );
            var newRefreshToken = _jwtAthenticationService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync( user );
            UserAuthenticationResponse authenticationResponse = new UserAuthenticationResponse( new JwtSecurityTokenHandler().WriteToken( newAccessToken ), newRefreshToken, newAccessToken.ValidTo );
            return authenticationResponse;
        }
    }
}
