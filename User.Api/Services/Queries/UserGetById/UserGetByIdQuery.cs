
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;
using UserApp.Api.Exceptions;
using MediatR;
using UserApp.Api.Models;

namespace UserApp.Api.Services.Queries.UserGetById
{
    public class UserGetByIdQuery : IRequest<UserResponseModel>
    {
        public UserGetByIdQuery(
            string id
        )
        {
            Id = id;
        }

        public string Id { get; }
    }
}
