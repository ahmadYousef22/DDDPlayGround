using DDDPlayGround.Host;
using DDDPlayGround.Infrastructure;
using DDDPlayGround.Infrastructure.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConnectionString(builder.Configuration);

builder.Services.AddSwaggerAuthentication();

builder.Services.AddJwtAuthentication(builder.Configuration); 

builder.Services.AddDependancyInjection();

builder.Host.AddLoggingService();

builder.Services.AddMapperService();

builder.Services.AddCorsService();

builder.Services.AddRateLimiting(builder.Configuration);

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();

app.UseCors("AllowAll");

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDPlayGround API V1");
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();