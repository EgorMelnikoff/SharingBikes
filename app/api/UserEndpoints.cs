using sharing_bikes.net.dto;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.interfaces;

namespace sharing_bikes.net.api;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/users").WithTags("Users");

        group.MapGet("/", async (IUserService users, IMapper mapper) =>
            {
                var result = await users.GetAll();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список юзеров")
            .WithDescription("Возвращает всех зарегистрированных юзеров")
            .Produces<IEnumerable<UserResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{userId:guid}",
                async (Guid userId, IUserService users, IMapper mapper) =>
            {
                var user = await users.GetUserById(userId);
                return user is null
                    ? Results.NotFound(new ErrorResponse { Message = "Юзер не найден." })
                    : Results.Ok(mapper.Map(user));
            })
            .WithSummary("Получить юзера")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
        
        group.MapPost("/", async (CreateUserRequest body, IUserService users, IMapper mapper) =>
            {
                try
                {
                    var created = await users.CreateUser(body);
                    return Results.Created($"/api/users/{created.Id}", mapper.Map(created));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Добавить юзера")
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{userId:guid}",
                async (Guid userId, IUserService users) =>
            {
                try
                {
                    var deleted = await users.DeleteUser(userId);
                    return deleted
                        ? Results.NoContent()
                        : Results.NotFound(new ErrorResponse { Message = "Юзер не найден." });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Удалить юзера")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return api;
    }
}
