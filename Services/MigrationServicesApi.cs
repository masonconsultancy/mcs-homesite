using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Users;
using MCS.HomeSite.Handlers;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Services
{
    public static class MigrationServicesApi
    {

        public static RouteGroupBuilder MapMigrationServiceGroup(this RouteGroupBuilder group)
        {
            group.MapGet("/MigrateData",
                    async (IMigrationHandler handler) =>
                    {
                        await handler.Process(new CancellationTokenSource());
                        Results.Ok(await handler.Result());
                    }).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status204NoContent);

            group.MapGet("/GetUsers", async (McsHomeSiteDestContext context) =>
                    context.Users.Any() ?
                        Results.Ok(await context.Users.ToListAsync()) : Results.NoContent()).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status204NoContent);
            
            return group;
        }

    }
}
