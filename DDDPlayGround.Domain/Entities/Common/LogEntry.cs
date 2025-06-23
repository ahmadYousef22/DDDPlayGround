using DDDPlayGround.Shared.Base;
using DDDPlayGround.Shared.Enums;

namespace DDDPlayGround.Domain.Entities.Common
{
    public class LogEntry : AuditableEntity
    {
        public LogSeverity Level { get; private set; } = default!;
        public string Message { get; private set; } = default!;
        public string? Exception { get; private set; }
        public string? CorrelationId { get; private set; }
        public string? User { get; private set; }
 
        protected LogEntry() { }

        public LogEntry(LogSeverity level, string message, string? exception, string? correlationId, string? user)
        {
            Level = level;
            Message = message;
            Exception = exception;
            CorrelationId = correlationId;
            User = user;
        }
    }
}
