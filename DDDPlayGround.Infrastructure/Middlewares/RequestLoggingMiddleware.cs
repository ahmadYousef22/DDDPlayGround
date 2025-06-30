using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();  

        var request = context.Request;
        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
        _logger.LogInformation("Incoming request: {Method} {Path} Body: {Body}", request.Method, request.Path, requestBody);

        request.Body.Position = 0;

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        var trimmedResponse = TrimBody(responseText); // trimmed large response like files ... 

        _logger.LogInformation("Outgoing response: {StatusCode} Body: {ResponseBody}", context.Response.StatusCode, trimmedResponse);

        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private string TrimBody(string body, int maxLength = 2048)
    {
        return string.IsNullOrWhiteSpace(body)
            ? string.Empty
            : (body.Length <= maxLength ? body : body.Substring(0, maxLength) + "...[TRIMMED]");
    }
}
