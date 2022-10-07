using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using UserApp.Api.Exceptions;
using UserApp.Api.Models;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Queries.UserGetByEmail
{
    public class UserGetByEmailQueryHandler : IRequestHandler<UserGetByEmailQuery, UserResponseModel>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;

        public UserGetByEmailQueryHandler(
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            IDataProtectionProvider provider
        )
        {
            _userRepository = userRepository;
            this._mapper = mapper;
            _protector = provider.CreateProtector( "" );
        }

        public async Task<UserResponseModel> Handle( UserGetByEmailQuery request, CancellationToken cancellationToken )
        {
            User user = await _userRepository.GetByEmail( request.Email );
            if ( user is null )
            {
                throw new UserNotFoundError( $"User with email {request.Email} does not found" );
            }
            UserResponseModel userResponse = _mapper.Map<UserResponseModel>( user );
            userResponse.Id = _protector.Protect( user.Id.ToString() );
            return userResponse;
        }
    }
}
