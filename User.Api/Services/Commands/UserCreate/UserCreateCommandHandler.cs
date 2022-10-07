using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using UserApp.Api.Exceptions;
using UserApp.Api.Models;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Commands.UserCreate
{
    public class UserCreateCommnadHandler : IRequestHandler<UserCreateCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDataProtector _protector;

        public UserCreateCommnadHandler(
            IMapper mapper,
            IUserRepositoryAsync userRepository,
            IDataProtectionProvider provider,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            this._mapper = mapper;
            this._userRepository = userRepository;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._protector = provider.CreateProtector( "" );
        }
        public async Task<string> Handle( UserCreateCommand request, CancellationToken cancellationToken )
        {
            User user = _mapper.Map<User>( request.UserUpsertRequest );
            User existingUser = await _userRepository.GetByEmail( user.Email );
            if ( existingUser is not null )
            {
                throw new UserAlreadyExistsError( $"{user.Email} is not available" );
            }
            IdentityResult result = await _userManager.CreateAsync( user, request.UserUpsertRequest.Password );
            //Guid userId = await _userRepository.Create( user );
            if ( !result.Succeeded )
            {
                throw new ApplicationException( "Something went wrong" );
            }

            if ( !await _roleManager.RoleExistsAsync( request.UserRole ) )
            {
                await _roleManager.CreateAsync( new IdentityRole( request.UserRole ) );
            }

            var defaultrole = await _roleManager.FindByNameAsync( request.UserRole );
            if ( defaultrole != null )
            {
                IdentityResult roleresult = await _userManager.AddToRoleAsync( user, defaultrole.Name );
                if ( !roleresult.Succeeded )
                {
                    throw new ApplicationException( "Something went wrong" );
                }
            }
            return _protector.Protect( user.Id );
        }
    }
}
