using FluentValidation;
using MCS.HomeSite.Common;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Migration;
using MCS.HomeSite.Data.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Handlers
{
    public interface IMigrationHandler
    {
        Task Process(CancellationTokenSource cts);
        Task<IList<MigrationLogDto>> Result();
    }

    public class MigrationHandler : IMigrationHandler
    {
        private readonly McsHomeSiteDestContext _contextDestination;
        private readonly McsHomeSiteContext _contextSource;
        private readonly IValidator<UserDto> _validator;

        public MigrationHandler(McsHomeSiteDestContext contextDestination, McsHomeSiteContext contextSource, IValidator<UserDto> validator)
        {
            _contextDestination = contextDestination;
            _contextSource = contextSource;
            _validator = validator;
        }

        public async Task Process(CancellationTokenSource cts)
        {
            //Clean down tables before starting.
            //We might not want to do this for all tables.
            //We may need to get a list of missing data, missing items are added, existing items are updated.
            await _contextDestination.CleanTable(_contextDestination.MigrationLogs, progress: x => Task.CompletedTask);
            await _contextDestination.CleanTable(_contextDestination.Users, progress: x => Task.CompletedTask);
            await _contextDestination.SaveChangesWithAuditAsync(default, true).ConfigureAwait(false);

            //Perform Validation
            var successful = true;
            foreach (var contextSourceUser in _contextSource.Users)
            {
                var validatorContext = new ValidationContext<UserDto>(contextSourceUser);

                var validationResult = await _validator.ValidateAsync(validatorContext);

                if (validationResult.IsValid) continue;

                successful = false;

                foreach (var errorItem in validationResult.ToDictionary())
                {
                    _contextDestination.MigrationLogs.Add(new MigrationLogDto()
                    { Error = $"{errorItem.Key}:{string.Concat(errorItem.Value)}", Index = $"Id:{contextSourceUser.Id}", TableName = "UserDto" });
                }
            }

            if (!successful)
            {
                await _contextDestination.SaveChangesWithAuditAsync(default, true).ConfigureAwait(false);
                return;
            }

            //Begin the import of data in a transaction.
            var strategy = _contextDestination.Database.CreateExecutionStrategy();

            await _contextDestination.ExecuteInTransaction(
                _contextSource.Users,
                strategy, cts.Token, progress: x => Task.CompletedTask).ConfigureAwait(false);


        }

        public async Task<IList<MigrationLogDto>> Result()
        {
            return await _contextDestination.MigrationLogs.ToListAsync().ConfigureAwait(false);
        }
    }
}
