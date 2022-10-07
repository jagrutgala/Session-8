using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;
using UserApp.Api.Exceptions;
using MediatR;
using UserApp.Api.Models;

namespace UserApp.Api.Services.Queries.UserGetByEmail
{
    public class UserGetByEmailQuery : IRequest<UserResponseModel>
    {
        public UserGetByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
