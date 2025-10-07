using DDDPlayGround.Host;
using DDDPlayGround.Infrastructure;
using DDDPlayGround.Infrastructure.Configuration;
using System.Text.Encodings.Web;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConnectionString(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping; 
     });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerAuthentication();

builder.Services.AddJwtAuthentication(builder.Configuration); 

builder.Services.AddDependancyInjection();

builder.Host.AddLoggingService(builder.Configuration);
  
builder.Services.AddMapperService();

builder.Services.AddCorsService();

builder.Services.AddRateLimiting();

builder.Services.AddHttpClient();

builder.Services.AddHttpAccessor();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddFluentValidationServices();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
    });
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCustomSerilogRequestLogging();

app.UseSwagger();


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDPlayGround API V1");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

//if (builder.Environment.IsProduction()) { app.UseHsts(); } // this allow only https request 

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomExceptionMiddleware();

app.UseRateLimiter();

app.UseRequestLogging();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Participant}/{action=Index}/{id?}");

app.Run();