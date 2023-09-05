using MCS.HomeSite.Common;
using MCS.HomeSite.Data.Models;
using MCS.HomeSite.Data.Models.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MCS.HomeSite.Data
{
    public abstract class BaseDbContext<T> : DbContext where T : DbContext
    {
        private readonly string? _connectionString;
        private readonly int _commandTimeout;
        private readonly int _enableRetryOnFailure;
        private readonly bool _canAudit;
        private readonly IHttpContextAccessor? _accessor;

        public static bool TryCreateNew(string connectionString, int commandTimeout, int enableRetryOnFailure, bool canAudit, out T? context)
        {
            try
            {
                var instance = Activator.CreateInstance(typeof(T), new object[] { connectionString, commandTimeout, enableRetryOnFailure, canAudit });
                if (instance == null)
                {
                    context = null;
                    return false;
                }
                context = (T)instance;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetFullMessage());
                context = default;
                return false;
            }
        }

        protected BaseDbContext([NotNull] DbContextOptions<T> options, IConfiguration config, IHttpContextAccessor accessor) : base(options)
        {
            _canAudit = Convert.ToBoolean(config["Auditing:AuditingEnabled"] ?? "false");
            _accessor = accessor;
        }

        protected BaseDbContext(string connectionString, int commandTimeout, int enableRetryOnFailure, bool canAudit)
        {
            _connectionString = connectionString;
            _commandTimeout = commandTimeout;
            _enableRetryOnFailure = enableRetryOnFailure;
            _canAudit = canAudit;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
                return;
            }

            if (string.IsNullOrEmpty(_connectionString) || _connectionString == "none")
            {
                optionsBuilder.UseInMemoryDatabase(nameof(T));
                optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                optionsBuilder.EnableSensitiveDataLogging();
            }
            else
            {

                optionsBuilder.UseSqlServer(_connectionString, x =>
                {
                    x.EnableRetryOnFailure(_enableRetryOnFailure);
                    x.CommandTimeout(_commandTimeout);
                });

            }
            base.OnConfiguring(optionsBuilder);
        }

        public async Task<BaseResponse> SaveChangesWithAuditAsync(CancellationToken cancellationToken = default, bool autoSaveAuditLogs = true)
        {
            var auditLogs = new List<AuditLogDto>();
            GenerateChanges(ChangeTracker, out var modifiedRemovedTrackedEntities, out var addedTrackedEntities);
            LogChanges(modifiedRemovedTrackedEntities, ref auditLogs);
            var saveResult = await base.SaveChangesAsync(cancellationToken);
            LogChanges(addedTrackedEntities, ref auditLogs, EntityState.Added);
            if (!autoSaveAuditLogs) return new BaseResponse { StateEntriesWritten = saveResult, AuditLogs = auditLogs };
            await base.AddRangeAsync(auditLogs, cancellationToken).ConfigureAwait(false);
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new BaseResponse { StateEntriesWritten = saveResult, AuditLogs = auditLogs };
        }

        private void GenerateChanges(ChangeTracker changeTracker,
            out IEnumerable<EntityEntry> modifiedRemovedTrackedEntities,
            out IEnumerable<EntityEntry> addedTrackedEntities)
        {
            if (!_canAudit)
            {
                addedTrackedEntities = null;
                modifiedRemovedTrackedEntities = null;
                return;
            }
            addedTrackedEntities = new List<EntityEntry>(changeTracker.Entries().Where(x => x.State == EntityState.Added));
            modifiedRemovedTrackedEntities = new List<EntityEntry>(changeTracker.Entries().Where(x => x.State is EntityState.Modified or EntityState.Deleted));
        }

        private void LogChanges(IEnumerable<EntityEntry> changes, ref List<AuditLogDto> auditLogs, EntityState? entityState = null)
        {
            auditLogs ??= new List<AuditLogDto>();

            if (!_canAudit || changes == null)
            {
                return;
            }

            foreach (var change in changes)
            {
                var canAuditDbSet = AuditingAttribute.CanAuditDbSet(change.Entity.GetType());

                if (!canAuditDbSet.AuditDbSet)
                    continue;

                var originalValues = change.GetDatabaseValues();
                var currentValues = change.CurrentValues;
                var keys = GetKeys(change.Entity);
                var entityName = change.Entity.GetType().Name;
                var currentEntityState = entityState ?? change.State;

                switch (currentEntityState)
                {
                    case EntityState.Added when canAuditDbSet.RecordAdded: //Log Added
                        auditLogs.Add(new AuditLogDto
                        {
                            EntityKeyValue = Convert.ToInt64(keys["Id"]),
                            EntityName = entityName,
                            EntityState = EntityState.Added,
                            AdditionalData = currentValues.GetPropertyValue<string>(canAuditDbSet.AdditionalDataField),
                            ParentEntityKeyValue = currentValues.GetPropertyValue<long?>(canAuditDbSet.ParentEntityKeyField)
                        });
                        break;
                    case EntityState.Modified:
                        {
                            //Log Modified
                            auditLogs.AddRange(from property in originalValues.Properties
                                               where !canAuditDbSet.Fields.Any() || canAuditDbSet.Fields.Contains(property.Name)
                                               let originalValue = originalValues.GetPropertyValue<string>(property.Name)
                                               let currentValue = currentValues.GetPropertyValue<string>(property.Name)
                                               where !Equals(originalValue, currentValue)
                                               select new AuditLogDto
                                               {
                                                   EntityKeyValue = Convert.ToInt64(keys["Id"]),
                                                   EntityName = entityName,
                                                   PropertyName = property.Name,
                                                   OriginalValue = originalValue,
                                                   CurrentValue = currentValue,
                                                   EntityState = EntityState.Modified,
                                                   AdditionalData = currentValues.GetPropertyValue<string>(canAuditDbSet.AdditionalDataField),
                                                   ParentEntityKeyValue = currentValues.GetPropertyValue<long?>(canAuditDbSet.ParentEntityKeyField)
                                               });
                            break;
                        }
                    case EntityState.Deleted when canAuditDbSet.RecordDeleted:
                        //Log Removed
                        auditLogs.Add(new AuditLogDto
                        {
                            EntityKeyValue = Convert.ToInt64(keys["Id"]),
                            EntityName = entityName,
                            EntityState = EntityState.Deleted,
                            PropertyName = canAuditDbSet.RecordFieldOnDelete,
                            OriginalValue = currentValues.GetPropertyValue<string>(canAuditDbSet.RecordFieldOnDelete),
                            AdditionalData = currentValues.GetPropertyValue<string>(canAuditDbSet.AdditionalDataField),
                            ParentEntityKeyValue = currentValues.GetPropertyValue<long?>(canAuditDbSet.ParentEntityKeyField)
                        });
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                }
            }
        }

        private Dictionary<string, object> GetKeys<TEntity>(TEntity entity)
        {
            var keyValues = new Dictionary<string, object>();
            var entityType = Model.FindEntityType(entity.GetType());
            var keyNames = entityType?.FindPrimaryKey()?.Properties
                .Select(x => x.Name) ?? Array.Empty<string>();
            foreach (var keyName in keyNames)
            {
                var value = entity.GetType().GetProperty(keyName)?.GetValue(entity, null);
                keyValues.Add(keyName, value);
            }
            return keyValues;
        }
    }
}
