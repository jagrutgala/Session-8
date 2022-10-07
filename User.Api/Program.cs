
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using UserApp.Api;
using UserApp.Api.Mapper;
using UserApp.Infrastructure.DatabaseContexts;
using UserApp.Infrastructure.Repositories;

internal class Program
{
    private static void Main( string[] args )
    {
        var builder = WebApplication.CreateBuilder( args );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        Logger logger = builder.Services.AddSerilogLogger();
        builder.Host.UseSerilog( logger );

        builder.Services.AddSwaggerVersionedApiExplorer();

        builder.Services.AddPresentationServices();

        string connectionString = builder.Configuration.GetConnectionString( "DefaultConnection" );
        builder.Services
            .AddDbContext<ApplicationDbContext>( options => options.UseSqlServer( connectionString ) );

        builder.Services.AddAuthenticationServices( builder.Configuration );


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if ( app.Environment.IsDevelopment() )
        {
            app.UseSwagger();
            app.UseSwaggerUI(
               options =>
               {
                   IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                   foreach ( var description in provider.ApiVersionDescriptions )
                   {
                       options.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );
                   }
               } );
        }


        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}