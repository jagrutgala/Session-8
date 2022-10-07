using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using UserApp.Api.Exceptions;
using UserApp.Api.Models;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Queries.UserGetById
{
    public class UserGetByIdCommnadHandler : IRequestHandler<UserGetByIdQuery, UserResponseModel>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;

        public UserGetByIdCommnadHandler(
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            IDataProtectionProvider provider
        )
        {
            _userRepository = userRepository;
            this._mapper = mapper;
            _protector = provider.CreateProtector( "" );
        }
        public async Task<UserResponseModel> Handle( UserGetByIdQuery request, CancellationToken cancellationToken )
        {
            User user = await _userRepository.GetById( _protector.Unprotect( request.Id ) );
            if ( user is null )
            {
                throw new UserNotFoundError( $"User with id {request.Id} not found" );
            }
            UserResponseModel userResponse = _mapper.Map<UserResponseModel>( user );
            userResponse.Id = _protector.Protect( user.Id );
            return userResponse;
        }
    }
}
