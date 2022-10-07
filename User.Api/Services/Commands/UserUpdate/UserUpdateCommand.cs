using AutoMapper;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;
using UserApp.Api.Exceptions;
using MediatR;
using UserApp.Api.Models;

namespace UserApp.Api.Services.Commands.UserUpdate
{
    public class UserUpdateCommand : IRequest<UserResponseModel>
    {
        public UserUpdateCommand(UserUpdateRequestModel userUpsertRequest)
        {
            UserUpdateRequest = userUpsertRequest;
        }

        public UserUpdateRequestModel UserUpdateRequest { get; }
    }


}
