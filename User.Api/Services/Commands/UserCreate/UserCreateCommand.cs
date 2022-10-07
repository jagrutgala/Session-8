using MediatR;
using UserApp.Api.Models;
using UserApp.Infrastructure.Entities;

namespace UserApp.Api.Services.Commands.UserCreate
{
    public class UserCreateCommand : IRequest<string>
    {
        public UserCreateCommand(
            UserInsertRequestModel userUpsertRequest,
            string userRole = UserRoles.User
        )
        {
            UserUpsertRequest = userUpsertRequest;
            UserRole = userRole;
        }

        public UserInsertRequestModel UserUpsertRequest { get; }
        public string UserRole { get; }
    }
}
