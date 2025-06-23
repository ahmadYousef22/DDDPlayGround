using DDDPlayGround.Application.Authentication;
using DDDPlayGround.Application.Authentication.JwtToken;
using DDDPlayGround.Application.Integration;
using DDDPlayGround.Application.Services;
using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Infrastructure.Integration;
using DDDPlayGround.Infrastructure.Persistence.Context;
using DDDPlayGround.Infrastructure.Repositories;
using DDDPlayGround.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
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
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IRestIntegrationService, RestIntegrationService>();
            services.AddScoped<INumberConversionService, NumberConversionService>();


            return services;
        }
        public static IHostBuilder AddLoggingService(this IHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateLifetime = true
                };
            });

            return services;
        }
    }
}
