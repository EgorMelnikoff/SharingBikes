using sharing_bikes.net.dto;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.interfaces;

namespace sharing_bikes.net.api;

public static class VehicleEndpoints
{
    public static RouteGroupBuilder MapVehiclesEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IVehicleService vehicles, IMapper mapper) =>
                {
                    var result = await vehicles.GetAll();
                    return Results.Ok(result.Select(mapper.Map));
                }
            )
            .WithSummary("Получить список самокатов")
            .WithDescription("Возвращает всех самокатов")
            .Produces<IEnumerable<VehicleResponse>>();

        group.MapGet("/{vehicleId:guid}", async (Guid vehicleId, IVehicleService vehicles, IMapper mapper) =>
                {
                    var vehicle = await vehicles.GetVehicleById(vehicleId);
                    return vehicle is null
                        ? Results.NotFound(new ErrorResponse { Message = "Самокат не найден." })
                        : Results.Ok(mapper.Map(vehicle));
                }
            )
            .WithSummary("Получить самокат по ID")
            .Produces<VehicleResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateVehicleRequest body, IVehicleService vehicles, IMapper mapper) =>
                {
                    try
                    {
                        var created = await vehicles.CreateVehicle(body);
                        return Results.Created($"/api/vehicles/{created.Id}", mapper.Map(created));
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                    }
                }
            )
            .WithSummary("Добавить самокат")
            .Produces<VehicleResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapPatch("/{vehicleId:guid}", async (
                    Guid vehicleId,
                    PatchVehicleRequest body,
                    IVehicleService vehicles,
                    IMapper mapper
                ) =>
                {
                    try
                    {
                        var updated = await vehicles.PatchVehicle(vehicleId, body);
                        return updated is null
                            ? Results.NotFound(new ErrorResponse { Message = "Самокат не найден." })
                            : Results.Ok(mapper.Map(updated));
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                    }
                }
            )
            .WithSummary("Обновить самокат (частично)")
            .Produces<VehicleResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapDelete("/{vehicleId:guid}", async (Guid vehicleId, IVehicleService vehicles) =>
                {
                    try
                    {
                        var deleted = await vehicles.DeleteVehicle(vehicleId);
                        return deleted
                            ? Results.NoContent()
                            : Results.NotFound(new ErrorResponse { Message = "Самокат не найден." });
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                    }
                }
            )
            .WithSummary("Удалить самокат")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return api;
    }
}