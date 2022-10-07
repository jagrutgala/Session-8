
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using UserApp.Api.Exceptions;
using UserApp.Api.Models;
using UserApp.Api.Services.Commands;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Commands.UserUpdate
{
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, UserResponseModel>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;

        public UserUpdateCommandHandler(
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            IDataProtectionProvider provider
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _protector = provider.CreateProtector( "" );
        }

        public async Task<UserResponseModel> Handle( UserUpdateCommand request, CancellationToken cancellationToken )
        {
            User user = _mapper.Map<User>( request.UserUpdateRequest );
            user.Id = _protector.Unprotect( request.UserUpdateRequest.Id );
            User existingUser = await _userRepository.GetById( user.Id );
            if ( existingUser is null )
            {
                throw new UserNotFoundError( $"User with id={user.Id} does not exist" );
            }
            _mapper.Map<User, User>(user, existingUser);
            User updatedUser = await _userRepository.Update( existingUser );
            UserResponseModel userResponse = _mapper.Map<UserResponseModel>( updatedUser );
            userResponse.Id = _protector.Protect( user.Id.ToString() );
            return userResponse;
        }
    }
}
