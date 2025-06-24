using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DDDPlayGround.Infrastructure.Persistence.Interceptors
{
    internal class HandleAuditInterceptor : SaveChangesInterceptor
    {
        //public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        //{
        //    var context = eventData.Context;
        //    context.ChangeTracker.Entries<AuditableEntity>().Select(x =>
        //    {
        //        x.Entity.SetCreated()
        //    })
        //}
    }
}
