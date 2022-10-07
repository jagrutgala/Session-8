using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using UserApp.Api.Models;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api.Services.Queries.UserGetAll
{
    public class UserGetAllQueryHandler : IRequestHandler<UserGetAllQuery, IEnumerable<UserResponseModel>>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;

        public UserGetAllQueryHandler(
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            IDataProtectionProvider provider
        )
        {
            _userRepository = userRepository;
            this._mapper = mapper;
            _protector = provider.CreateProtector( "" );
        }

        public async Task<IEnumerable<UserResponseModel>> Handle( UserGetAllQuery request, CancellationToken cancellationToken )
        {
            List<User> users = (List<User>) await _userRepository.GetAll();
            List<UserResponseModel> userResponses = _mapper.Map<List<UserResponseModel>>( users );
            for ( int i = 0; i < users.Count; i++ )
            {
                userResponses[i].Id = _protector.Protect( users[i].Id.ToString() );
            }
            return userResponses;
        }
    }
}
