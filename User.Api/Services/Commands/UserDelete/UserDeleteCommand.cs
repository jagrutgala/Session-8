using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;
using UserApp.Api.Exceptions;
using MediatR;

namespace UserApp.Api.Services.Commands.UserDelete
{
    public class UserDeleteCommand : IRequest
    {
        public UserDeleteCommand(
            string userid
        )
        {
            UserId = userid;
        }

        public string UserId { get; }
    }
}
