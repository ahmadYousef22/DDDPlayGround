namespace DDDPlayGround.Application.Services
{
    public interface ILoggingService<T>
    {
        Task LogInformationAsync(string message, string? correlationId = null, string? user = null);
        Task LogWarningAsync(string message, string? correlationId = null, string? user = null);
        Task LogErrorAsync(string message, Exception? exception = null, string? correlationId = null, string? user = null);
    }
}
