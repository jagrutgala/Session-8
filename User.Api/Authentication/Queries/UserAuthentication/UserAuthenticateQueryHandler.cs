using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserApp.Api.Authentication.Common;
using UserApp.Api.Exceptions;
using UserApp.Infrastructure.Entities;

namespace UserApp.Api.Authentication.Queries.UserAuthentication
{
    public class UserAuthenticateQueryHandler : IRequestHandler<UserAuthenticateQuery, UserAuthenticationResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtAthenticationService _jwtService;
        private RoleManager<IdentityRole> _roleManager { get; }
        public UserAuthenticateQueryHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtAthenticationService jwtService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        public async Task<UserAuthenticationResponse> Handle(UserAuthenticateQuery request, CancellationToken cancellationToken)
        {
            User user;
            string email = request.AuthenticationRequest.Email;
            string passowrd = request.AuthenticationRequest.Password;
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidAuthenticationRequestCredentials();
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, passowrd, false, true);
            if (!result.Succeeded)
            {
                throw new InvalidAuthenticationRequestCredentials();
            }
            user.LastLoginTime = DateTime.Now;
            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new InvalidAuthenticationRequestCredentials();
            }
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            claims.Add( new Claim( ClaimTypes.Name, user.UserName ) );
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken token = _jwtService.GenerateToken(claims);
            string refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(60);
            await _userManager.UpdateAsync(user);

            UserAuthenticationResponse authenticationResponse = new UserAuthenticationResponse(new JwtSecurityTokenHandler().WriteToken(token), refreshToken, token.ValidTo);
            return authenticationResponse;
        }
    }
}
