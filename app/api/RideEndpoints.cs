using sharing_bikes.net.dto;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.interfaces;

namespace sharing_bikes.net.api;

public static class RideEndpoints
{
    public static RouteGroupBuilder MapRidesEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/rides").WithTags("Rides");

        group.MapGet("/", async (IRideService rides, IMapper mapper) =>
            {
                var result = await rides.GetAll();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список поездок")
            .WithDescription("Возвращает всех поездок.")
            .Produces<IEnumerable<RideResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{rideId:guid}",
                async (Guid rideId, IRideService rides, IMapper mapper) =>
                {
                    var ride = await rides.GetRideById(rideId);
                    return ride is null
                        ? Results.NotFound(new ErrorResponse { Message = "Клиент не найден." })
                        : Results.Ok(mapper.Map(ride));
                })
            .WithSummary("Получить бонусный баланс клиента")
            .WithDescription("Возвращает количество накопленных бонусов по идентификатору клиента.")
            .Produces<RideResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/start", async (StartRideRequest body, IRideService rides, IMapper mapper) =>
            {
                try
                {
                    var created = await rides.StartRide(body);
                    
                    return Results.Created($"/api/rides/{created.Id}", created);
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Начать поездку")
            .Produces<StartRideResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapPost("/end", async (EndRideRequest body, IRideService rides, IMapper mapper) =>
            {
                try
                {
                    var updated = await rides.EndRide(body);
                    return Results.Ok(mapper.Map(updated));
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Завершить поездку")
            .Produces<RideResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return api;
    }
}