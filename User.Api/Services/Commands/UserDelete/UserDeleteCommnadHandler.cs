using MediatR;
using Microsoft.AspNetCore.DataProtection;
using UserApp.Api.Exceptions;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Commands.UserDelete
{
    public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IDataProtector _protector;

        public UserDeleteCommandHandler(
            IUserRepositoryAsync userRepository,
            IDataProtectionProvider provider
        )
        {
            _userRepository = userRepository;
            _protector = provider.CreateProtector( "" );
        }

        public async Task<Unit> Handle( UserDeleteCommand request, CancellationToken cancellationToken )
        {
            
            User user = await _userRepository.GetById( _protector.Unprotect( request.UserId ) );
            if ( user is null )
            {
                throw new UserNotFoundError( $"User with id={request.UserId} does not exist" );
            }
            user.IsDeleted = true;
            user = await _userRepository.Update( user );
            return await Task.FromResult( Unit.Value );
        }
    }
}
