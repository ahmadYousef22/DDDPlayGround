using DDDPlayGround.Host;
using DDDPlayGround.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConnectionString(builder.Configuration);

builder.Services.AddSwaggerAuthentication();

builder.Services.AddJwtAuthentication(builder.Configuration); 

builder.Services.AddDependancyInjection();

builder.Host.AddLoggingService(builder.Configuration);
  
builder.Services.AddMapperService();

builder.Services.AddCorsService();

builder.Services.AddRateLimiting(builder.Configuration);

builder.Services.AddHttpClient();

builder.Services.AddHttpAccessor();

var app = builder.Build();

app.UseCustomSerilogRequestLogging();

app.UseSwagger();

app.UseCors("AllowAll");

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDPlayGround API V1");
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomExceptionMiddleware();

app.UseRequestLogging();

app.MapControllers();

app.Run();