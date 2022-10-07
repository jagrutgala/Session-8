using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserApp.Api.Controllers;
using UserApp.Api.Models;
using UserApp.Api.Services.Queries.UserGetAll;
using UserApp.Api.Services.Queries.UserGetById;
using UserApp.Infrastructure.Repositories;

namespace Test.UserApp.Api
{
    public class UserControllerTest
    {
        private readonly UserController userController;
        private readonly Mock<ILogger<UserController>> logger;
        private readonly Mock<IMediator> mediator;
        List<UserResponseModel> userResponses;

        public UserControllerTest()
        {
            mediator = new Mock<IMediator>();
            logger = new Mock<ILogger<UserController>>();
            userController = new UserController( mediator.Object, logger.Object );

            userResponses = new List<UserResponseModel>()
                    {
                        new UserResponseModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = "Api",
                            Email="Api@gmail.com",
                            LastLoginTime = DateTime.Now
                        },
                        new UserResponseModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = "Bpi",
                            Email="Bpi@gmail.com",
                            LastLoginTime = DateTime.Now
                        },
                        new UserResponseModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = "Cpi",
                            Email="Cpi@gmail.com",
                            LastLoginTime = DateTime.Now
                        },
                    };

        }
        [Fact]
        public async void AllUsersTest()
        {
            // Extpected
            ObjectResult expected = new ObjectResult( new FormattedResponseModel( true, userResponses, null ) );
            
            // Mock
            UserGetAllQuery query = new UserGetAllQuery();
            mediator.Setup(
                m => m.Send( query, It.IsAny<CancellationToken>() )
            ).ReturnsAsync( (IEnumerable<UserResponseModel>) userResponses );

            // Act
            IActionResult result = await userController.Index();
            
            // Test
            Assert.IsAssignableFrom<IActionResult>( result );
            Assert.Equal( JsonSerializer.Serialize( expected ), JsonSerializer.Serialize( result ) );
        }

        [Fact]
        public async void UserGetByIdTest()
        {
            ObjectResult expected = new ObjectResult( new FormattedResponseModel( true, userResponses[0], null ) );
            UserGetByIdQuery query = new UserGetByIdQuery( userResponses[0].Id );
            mediator.Setup(
                m => m.Send( query, It.IsAny<CancellationToken>() )
            ).Returns( Task.FromResult( userResponses[0] ) );

            ObjectResult result = (ObjectResult) await userController.GetById( userResponses[0].Id );
            Assert.IsAssignableFrom<IActionResult>( result );
            Assert.Equal( JsonSerializer.Serialize( expected ), JsonSerializer.Serialize( result ) );
        }
    }
}