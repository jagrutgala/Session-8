
using AutoMapper;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;
using UserApp.Api.Exceptions;
using MediatR;
using UserApp.Api.Models;

namespace UserApp.Api.Services.Queries.UserGetAll
{
    public class UserGetAllQuery : IRequest<IEnumerable<UserResponseModel>>
    {

    }
}
