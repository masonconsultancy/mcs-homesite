using AutoMapper;
using MCS.HomeSite.Areas.DataTables.Data;
using MCS.HomeSite.Areas.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Services
{
    public static class UserServices
    {

        public static RouteGroupBuilder MapUserServiceGroup(this RouteGroupBuilder group)
        {
            group.MapGet("/GetUsers", async (McsHomeSiteContext context) => 
                context.UserDto.Any() ? 
                    Results.Ok(await context.UserDto.ToListAsync()) : Results.NoContent()).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status204NoContent);

            group.MapGet("/GetUser/{id:long?}", async (long? id, McsHomeSiteContext context) =>
                {
                    if (id == null)
                    {
                        return Results.BadRequest();
                    }
                    var userDto = await context.UserDto.FindAsync(id);
                    return userDto == null ? Results.NotFound() : Results.Ok(userDto);
                }).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status404NotFound).
                Produces<UserResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("/RemoveUser/{id:long?}", async (long? id, McsHomeSiteContext context) =>
                {
                    var userDto = await context.UserDto.FindAsync(id);

                    if (userDto == null)
                    {
                        return Results.NotFound();
                    }

                    context.UserDto.Remove(userDto);
                    await context.SaveChangesAsync();

                    return Results.NoContent();

                }).Produces<UserResponse>(StatusCodes.Status404NotFound)
                .Produces<UserResponse>(StatusCodes.Status204NoContent);

            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            group.MapPut("/UpdateUser/{id:long?}", async (long? id, User user, McsHomeSiteContext context, IMapper mapper) =>
            {
                if (id != user.Id)
                {
                    return Results.BadRequest();
                }

                context.Entry(mapper.Map<UserDto>(user)).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var userDtoExists = (context.UserDto?.Any(e => e.Id == id)).GetValueOrDefault();
                    if (!userDtoExists)
                    {
                        return Results.NotFound();
                    }
                    throw;
                }

                return Results.NoContent();
            }).Produces<UserResponse>(StatusCodes.Status400BadRequest).
                Produces<UserResponse>(StatusCodes.Status404NotFound).
                Produces<UserResponse>(StatusCodes.Status204NoContent);

            group.MapPost("/AddUser", async (User? user, McsHomeSiteContext context, IMapper mapper) =>
            {
                if (user == null)
                {
                    return Results.BadRequest();
                }
                var userDto = context.UserDto.Add(mapper.Map<UserDto>(user));
                await context.SaveChangesAsync();
                return Results.Ok(userDto);
            }).Produces<UserResponse>(StatusCodes.Status400BadRequest).
                Produces<UserResponse>();

            return group;
        }

    }
}
