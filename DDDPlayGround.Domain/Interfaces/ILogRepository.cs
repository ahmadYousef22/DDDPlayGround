using DDDPlayGround.Domain.Entities.Common;

namespace DDDPlayGround.Domain.Interfaces
{
    public interface ILogRepository
    {
        Task AddAsync(LogEntry entry);
        Task<IEnumerable<LogEntry>> GetRecentLogsAsync(int count);
        Task DeleteOldLogsAsync(DateTime beforeDate);
    }
}
