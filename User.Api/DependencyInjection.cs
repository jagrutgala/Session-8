using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using UserApp.Api.Authentication;
using UserApp.Api.Mapper;
using UserApp.Infrastructure.DatabaseContexts;
using UserApp.Infrastructure.Entities;
using UserApp.Infrastructure.Repositories;

namespace UserApp.Api
{
    public static class DependencyInjection
    {

        public static void AddPresentationServices( this IServiceCollection services )
        {
            services.AddScoped<IUserRepositoryAsync, UserSqlAsyncRepositry>();
            services.AddDataProtection();
            services.AddAutoMapper( typeof( UserProfile ) );
            services.AddMediatR( typeof( Program ).Assembly );
        }

        public static void AddAuthenticationServices( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddScoped<IJwtAthenticationService, JwtAthenticationService>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication( authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
)
            .AddJwtBearer( bearerOptions =>
            {
                JwtAthenticationService jwtAuthService = new JwtAthenticationService( configuration );
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.TokenValidationParameters = jwtAuthService.GetTokenValidationParameters();
            }
            );
        }


        public static Logger AddSerilogLogger( this IServiceCollection services )
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" )}.json",
                    optional: true
                )
                .Build();

            Logger logger = new LoggerConfiguration()
                .ReadFrom.Configuration( configuration )
                .Enrich.FromLogContext()
                .CreateLogger();

            return logger;
        }

        public static void AddSwaggerVersionedApiExplorer( this IServiceCollection services )
        {
            services.AddApiVersioning( options =>
            {
                options.DefaultApiVersion = new ApiVersion( 1, 0 );
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            } );

            services.AddVersionedApiExplorer( options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            } );

        }
    }
}
