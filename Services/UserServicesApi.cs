using AutoMapper;
using FluentValidation;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Services
{
    public static class UserServicesApi
    {

        public static RouteGroupBuilder MapUserServiceGroup(this RouteGroupBuilder group)
        {

            group.MapGet("/GetUsers", async (McsHomeSiteContext context) =>
                context.Users.Any() ?
                    Results.Ok(await context.Users.ToListAsync()) : Results.NoContent()).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status204NoContent);

            group.MapGet("/GetUser/{id:long?}", async (long? id, McsHomeSiteContext context) =>
                {
                    if (id == null)
                    {
                        return Results.BadRequest();
                    }
                    var userDto = await context.Users.FindAsync(id);
                    return userDto == null ? Results.NotFound() : Results.Ok(userDto);
                }).
                Produces<UserResponse>().
                Produces<UserResponse>(StatusCodes.Status404NotFound).
                Produces<UserResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("/RemoveUser/{id:long?}", async (long? id, McsHomeSiteContext context) =>
                {
                    var userDto = await context.Users.FindAsync(id);

                    if (userDto == null)
                    {
                        return Results.NotFound();
                    }

                    context.Users.Remove(userDto);
                    await context.SaveChangesWithAuditAsync(default, true);

                    return Results.NoContent();

                }).Produces<UserResponse>(StatusCodes.Status404NotFound)
                .Produces<UserResponse>(StatusCodes.Status204NoContent);

            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            group.MapPut("/UpdateUser/{id:long?}", async (long? id, User? user, McsHomeSiteContext context, IMapper mapper, IValidator<UserDto> validator) =>
            {
                if (user == null || id != user.Id)
                {
                    return Results.BadRequest();
                }

                var userDto = mapper.Map<UserDto>(user);

                var validationResult = await validator.ValidateAsync(userDto).ConfigureAwait(false);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                context.Entry(userDto).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesWithAuditAsync(default, true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var userDtoExists = (context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
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

            group.MapPost("/AddUser", async (User? user, McsHomeSiteContext context, IMapper mapper, IValidator<UserDto> validator) =>
                {

                    if (user == null)
                    {
                        return Results.BadRequest();
                    }

                    var userDto = mapper.Map<UserDto>(user);

                    var validationResult = await validator.ValidateAsync(userDto).ConfigureAwait(false);

                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    context.Users.Add(userDto);

                    await context.SaveChangesWithAuditAsync(default, true);
                    return Results.Ok(userDto);
                }).Produces<UserResponse>(StatusCodes.Status400BadRequest).
                Produces<UserResponse>();

            return group;
        }

    }
}
