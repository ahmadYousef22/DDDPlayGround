using DDDPlayGround.Domain.Entities.Common;
using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly DatabaseContext _dbContext;

        public LogRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(LogEntry entry)
        {
            await _dbContext.Set<LogEntry>().AddAsync(entry);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<LogEntry>> GetRecentLogsAsync(int count)
        {
            return await _dbContext.Set<LogEntry>()
                .OrderByDescending(log => log.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task DeleteOldLogsAsync(DateTime beforeDate)
        {
            var oldLogs = await _dbContext.Set<LogEntry>()
                .Where(log => log.CreatedDate < beforeDate)
                .ToListAsync();

            _dbContext.Set<LogEntry>().RemoveRange(oldLogs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
