using System.Collections.Immutable;
using FluentValidation;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MCS.HomeSite.Common
{
    internal static partial class Extensions
    {
        public static async Task UpdateRecords<TDbContext, TDbSet>(this TDbContext context,
            DbSet<TDbSet> dbSetfrom, Func<TDbSet, bool> predicate = null,
            Func<SyncDataProgress, Task> progress = null) where TDbContext : DbContext where TDbSet : class
        {            
            var recordsFrom = (predicate != null) ? dbSetfrom.AsNoTracking().Where(predicate).ToImmutableArray() : 
                dbSetfrom.AsNoTracking().ToImmutableArray();
            var recordCount = recordsFrom.Length;
            if (recordsFrom.Any())
                context.UpdateRange(recordsFrom);
            await progress?.Invoke(new SyncDataProgress { Position = recordCount, TotalCount = recordCount });
        }

        public static async Task UpdateRecords<TDbContext, TDbSet>(this TDbContext context, IEnumerable<TDbSet> dataFrom, 
            Func<SyncDataProgress, Task> progress = null) where TDbContext : DbContext where TDbSet : class
        {
            var recordCount = dataFrom.Count();
            if (dataFrom.Any())
                context.UpdateRange(dataFrom);
            await progress?.Invoke(new SyncDataProgress { Position = recordCount, TotalCount = recordCount });
        }

        public static async Task CleanTable<TDbContext, TDbSet>(
            this TDbContext context, 
            DbSet<TDbSet> dbSet, 
            Func<TDbSet, bool> predicate = null, 
            Func<SyncDataProgress,Task> progress = null) 
            where TDbContext : DbContext where TDbSet : class
        {
            var recordsFrom = (predicate != null) ? dbSet.AsNoTracking().Where(predicate).ToImmutableArray() : 
                dbSet.AsNoTracking().ToImmutableArray();
            var recordCount = recordsFrom.Count();
            context.RemoveRange(recordsFrom);
            await progress?.Invoke(new SyncDataProgress { Position = recordCount, TotalCount = recordCount });
        }

        public static async Task<bool> ValidateTable<TDbSet>(this DbSet<TDbSet> dbSet, IValidator validator,
            Func<TDbSet, bool> predicate = null, Func<MigrationLogDto, Task> progress = null) where TDbSet : class
        {
            //Perform Validation
            var successful = true;
            foreach (var contextSourceItem in dbSet)
            {
                
                var validatorContext = new ValidationContext<TDbSet>(contextSourceItem);

                var validationResult = await validator.ValidateAsync(validatorContext);

                if (validationResult.IsValid) continue;

                successful = false;

                foreach (var errorItem in validationResult.ToDictionary())
                {
                    progress?.Invoke(new MigrationLogDto { Error = $"{errorItem.Key}:{string.Concat(errorItem.Value)}", Index = $"Id", TableName = dbSet.EntityType.Name });
                    //_contextDestination.MigrationLogs.Add(new MigrationLogDto()
                    //    { Error = $"{errorItem.Key}:{string.Concat(errorItem.Value)}", Index = $"Id:{contextSourceUser.Id}", TableName = "UserDto" });
                }
            }
            return successful;
        }

        public static string GetTableName<TDbContext, TDbSet>(this TDbContext context) 
            where TDbContext : DbContext where TDbSet : class
        {
            var entityType = context.Model.FindEntityType(typeof(TDbSet));
            var schema = entityType.GetSchema();
            var tableName = entityType.GetTableName();
            return schema == null ? tableName : $"{schema}.{tableName}";
        }

        public static async Task AddRangeInBatchAsync<TDbContext, TDbSet>(this TDbContext context, DbSet<TDbSet> dbSet, 
            CancellationToken token, Func<TDbSet, bool> predicate = null, int batchNumber = 2400, 
            Func<SyncDataProgress,Task> progress = null) where TDbContext : DbContext where TDbSet : class
        {            
            var data = (predicate == null) ? dbSet.ToImmutableArray() : dbSet.Where(predicate).ToImmutableArray();
            var totalCount = data.Count();
            var position = 0;
            while (!token.IsCancellationRequested && position < totalCount)
            {
                await context.AddRangeAsync(data.Skip(position).Take(batchNumber), token);
                position += batchNumber;
                if (position > totalCount)
                    position = totalCount;

                progress?.Invoke(new SyncDataProgress { Position = position, TotalCount = totalCount });
            }
            _ = await (context as BaseDbContext<TDbContext>).SaveChangesWithAuditAsync(token, true).ConfigureAwait(false);
        }

        public static async Task ExecuteInTransaction<TDbContext, TDbSet>(this TDbContext context, DbSet<TDbSet> dbSet, IExecutionStrategy strategy, 
            CancellationToken token, Func<TDbSet, bool> predicate = null, 
            Func<SyncDataProgress,Task> progress = null) where TDbContext : DbContext where TDbSet : class
        {
            await strategy.ExecuteAsync(async
            () =>
            {
                var identityName = context.GetTableName<TDbContext, TDbSet>();
                await using var transaction = await context.Database.BeginTransactionAsync(token);
                await context.SetIdentityInsert(identityName, true, token);
                await context.AddRangeInBatchAsync(dbSet, token, predicate, progress: progress).ConfigureAwait(false);
                await context.SetIdentityInsert(identityName, false, token);
                await transaction.CommitAsync(token);
            });
        }

        public static async Task SetIdentityInsert<TDbContext>(this TDbContext context, string table, bool allow, 
            CancellationToken token) where TDbContext : DbContext
        {
            try
            {
                var allowStr = allow ? "ON" : "OFF";
                await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {table} {allowStr};", token).ConfigureAwait(false);
            }
            catch { }
        }
    }
}
