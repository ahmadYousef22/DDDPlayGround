using DDDPlayGround.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using DDDPlayGround.Shared.Enums;
using DDDPlayGround.Domain.Entities.Common;
using DDDPlayGround.Application.Services;

namespace DDDPlayGround.Infrastructure.Services
{
    public class LoggingService<T> : ILoggingService<T>
    {
        private readonly ILogger<T> _logger;
        private readonly ILogRepository _logRepository;

        public LoggingService(ILogger<T> logger, ILogRepository logRepository)
        {
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task LogInformationAsync(string message, string? correlationId = null, string? user = null)
        {
            _logger.LogInformation(message);
            var logEntry = new LogEntry(
                LogSeverity.Information,
                message,
                null,
                correlationId,
                user);

            await _logRepository.AddAsync(logEntry);

        }

        public async Task LogWarningAsync(string message, string? correlationId = null, string? user = null)
        {
            _logger.LogWarning(message);
            var logEntry = new LogEntry(
                LogSeverity.Warning,
                message,
                null,
                correlationId,
                user);

            await _logRepository.AddAsync(logEntry);
        }

        public async Task LogErrorAsync(string message, Exception? exception = null, string? correlationId = null, string? user = null)
        {
            _logger.LogError(exception, message);
            var logEntry = new LogEntry(
                LogSeverity.Error,
                message,
                exception?.ToString(),
                correlationId,
                user);

            await _logRepository.AddAsync(logEntry);
        }
    }
}
