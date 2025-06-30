using DDDPlayGround.Application.Authentication.JwtToken;
using DDDPlayGround.Application.Integration;
using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Infrastructure.Integration.Rest;
using DDDPlayGround.Infrastructure.Integration.Soap;
using DDDPlayGround.Infrastructure.Middlewares;
using DDDPlayGround.Infrastructure.Persistence.Context;
using DDDPlayGround.Infrastructure.Repositories;
using DDDPlayGround.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

namespace DDDPlayGround.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
            return services;
        }
        public static IServiceCollection AddDependancyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            //services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IRestIntegrationService, RestIntegrationService>();
            services.AddScoped<INumberConversionService, NumberConversionService>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
 

            return services;
        }
        public static IHostBuilder AddLoggingService(this IHostBuilder builder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "DDDPlayGround")
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                   connectionString: configuration.GetConnectionString("LoggingDb"),
                   sinkOptions: new MSSqlServerSinkOptions
                   {
                       TableName = "Logs",
                       AutoCreateSqlTable = true
                   })
                .CreateLogger();
            builder.UseSerilog();
            return builder;
        }        
        public static IServiceCollection AddMapperService(this IServiceCollection builder)
        {
            builder.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return builder;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true; // Optional: allows access to token in controller
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // no delay in expiration
                };
            });

            return services;
        }

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder application)
        {
            return application.UseMiddleware<ExceptionMiddleware>();
        }
        public static IServiceCollection AddHttpAccessor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor(); 
            return services;
        }
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var userName = httpContext.User?.Identity?.Name ?? "Anonymous";
                    var correlationId = httpContext.Request.Headers["X-Correlation-ID"].ToString();

                    diagnosticContext.Set("UserName", userName);
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("CorrelationId", correlationId);
                };
            });
        }
    }
}
