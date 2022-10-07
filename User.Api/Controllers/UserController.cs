using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApp.Api.Authentication.Common;
using UserApp.Api.Models;
using UserApp.Api.Services.Commands.UserCreate;
using UserApp.Api.Services.Commands.UserDelete;
using UserApp.Api.Services.Commands.UserUpdate;
using UserApp.Api.Services.Queries.UserGetAll;
using UserApp.Api.Services.Queries.UserGetByEmail;
using UserApp.Api.Services.Queries.UserGetById;
using UserApp.Infrastructure.Entities;
using UserApp.Api.Authentication.Queries.UserAuthentication;
using UserApp.Api.Authentication.Queries.RefreshToken;

namespace UserApp.Api.Controllers
{
    [ApiVersion( "1" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IMediator mediator,
            ILogger<UserController> logger
        )
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        [HttpGet, Route( "AllUsers" )]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation( $"{nameof( Index )} Initaited" );
            bool isSuccess = true;
            List<UserResponseModel> users = new List<UserResponseModel>();
            ApplicationException? exception = null;
            try
            {
                var my_var = await _mediator.Send( new UserGetAllQuery() );
                users = _mediator.Send( new UserGetAllQuery() ).Result.ToList();
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( Index )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, users, exception ) );
        }

        [HttpGet, Route( "[action]" )]
        public async Task<IActionResult> GetById( string id )
        {
            _logger.LogInformation( $"{nameof( GetById )} Initaited" );
            bool isSuccess = true;
            UserResponseModel? user = null;
            ApplicationException? exception = null;
            try
            {
                user = await _mediator.Send( new UserGetByIdQuery( id ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( GetById )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, user, exception ) );
        }


        [HttpGet, Route( "[action]" )]
        public async Task<IActionResult> GetbyEmail( string email )
        {
            _logger.LogInformation( $"{nameof( GetbyEmail )} Initaited" );
            bool isSuccess = true;
            UserResponseModel? user = null;
            ApplicationException? exception = null;
            try
            {
                user = await _mediator.Send( new UserGetByEmailQuery( email ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( GetbyEmail )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, user, exception ) );
        }

        [HttpPost, Route( "[action]" )]
        public async Task<IActionResult> Create( UserInsertRequestModel userUpsertRequest )
        {
            _logger.LogInformation( $"{nameof( Create )} Initaited" );
            bool isSuccess = true;
            string? userId = null;
            ApplicationException? exception = null;

            try
            {
                userId = await _mediator.Send( new UserCreateCommand( userUpsertRequest ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( Create )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, userId, exception ) );
        }

        [HttpPost, Route( "[action]" )]
        [Authorize]
        public async Task<IActionResult> Update( UserUpdateRequestModel userUpsertRequest )
        {
            _logger.LogInformation( $"{nameof( Update )} Initaited" );
            bool isSuccess = true;
            UserResponseModel? user = null;
            ApplicationException? exception = null;
            try
            {
                user = await _mediator.Send( new UserUpdateCommand( userUpsertRequest ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( Update )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, user, exception ) );
        }

        [HttpPost, Route( "[action]" )]
        [Authorize]
        public async Task<IActionResult> Delete( string id )
        {
            _logger.LogInformation( $"{nameof( Delete )} Initaited" );
            bool isSuccess = true;
            ApplicationException? exception = null;
            try
            {
                await _mediator.Send( new UserDeleteCommand( id ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( Delete )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, null, exception ) );
        }

        [HttpPost, Route( "Login" )]
        public async Task<IActionResult> Authenticate( UserAuthenticationRequest authenticationRequest )
        {
            bool isSuccess = true;
            UserAuthenticationResponse? authResponse = null;
            ApplicationException? exception = null;
            try
            {
                authResponse = await _mediator.Send( new UserAuthenticateQuery( authenticationRequest ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( Authenticate )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, authResponse, exception ) );
        }

        [HttpPost, Route( "[action]" )]
        public async Task<IActionResult> CreateAdmin( UserInsertRequestModel userUpsertRequest )
        {
            _logger.LogInformation( $"{nameof( CreateAdmin )} Initaited" );
            bool isSuccess = true;
            string? userId = null;
            ApplicationException? exception = null;

            try
            {
                userId = await _mediator.Send( new UserCreateCommand( userUpsertRequest, UserRoles.Admin ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( CreateAdmin )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, userId, exception ) );
        }

        [HttpPost, Route( "refreshToken" )]
        public async Task<IActionResult> RefreshToken( UserAuthenticationResponse authTokens )
        {
            _logger.LogInformation( $"{nameof( RefreshToken )} Initaited" );
            bool isSuccess = true;
            UserAuthenticationResponse? tokenResponse = null;
            ApplicationException? exception = null;
            try
            {
                tokenResponse = await _mediator.Send( new RefreshTokenQuery( authTokens ) );
            }
            catch ( ApplicationException err )
            {
                exception = err;
                isSuccess = false;
            }
            _logger.LogInformation( $"{nameof( RefreshToken )} Completed" );
            return StatusCode( StatusCodes.Status200OK, new FormattedResponseModel( isSuccess, tokenResponse, exception ) );

        }
    }

}
