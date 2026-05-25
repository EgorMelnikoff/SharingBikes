using sharing_bikes.net.dto;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.interfaces;

namespace sharing_bikes.net.api;

public static class FineEndpoints
{
    public static RouteGroupBuilder MapFinesEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/fines").WithTags("Fines");

        group.MapGet("/", async (IFineService fines, IMapper mapper) =>
            {
                var result = await fines.GetAll();
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список штрафов")
            .WithDescription("Возвращает все штрафы.")
            .Produces<IEnumerable<FineResponse>>();

        group.MapGet("/{fineId:guid}",
                async (Guid fineId, IFineService fines, IMapper mapper) =>
                {
                    var ride = await fines.GetFineById(fineId);
                    return ride is null
                        ? Results.NotFound(new ErrorResponse { Message = "Штраф не найден." })
                        : Results.Ok(mapper.Map(ride));
                }
            )
            .WithSummary("Получить штраф")
            .Produces<RideResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
        
      
        group.MapGet("/user/{userId:guid}", async (Guid userId, IFineService fines, IMapper mapper) =>
            {
                var result = await fines.GetActiveFineByUserId(userId);
                return Results.Ok(result.Select(mapper.Map));
            })
            .WithSummary("Получить список активных штрафов по юзеру")
            .Produces<IEnumerable<FineResponse>>();
        
        group.MapPost("/", async (CreateFineRequest body, IFineService fines, IMapper mapper) =>
            {
                try
                {
                    var created = await fines.CreateFine(body);
                    return Results.Created($"/api/fines/{created.Id}", mapper.Map(created));
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                {
                    return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                }
            })
            .WithSummary("Создать штраф")
            .Produces<FineResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        
        group.MapPatch("/{fineId:guid}", async (
                    Guid fineId,
                    PatchFineRequest body,
                    IFineService fines,
                    IMapper mapper
                ) =>
                {
                    try
                    {
                        var updated = await fines.PatchFine(fineId, body);
                        return updated is null
                            ? Results.NotFound(new ErrorResponse { Message = "Штраф не найден." })
                            : Results.Ok(mapper.Map(updated));
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                    }
                }
            )
            .WithSummary("Обновить штраф (частично)")
            .Produces<VehicleResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        
        group.MapDelete("/{fineId:guid}",
                async (Guid fineId, IFineService fines) =>
                {
                    try
                    {
                        var deleted = await fines.DeleteFIne(fineId);
                        return deleted
                            ? Results.NoContent()
                            : Results.NotFound(new ErrorResponse { Message = "Штраф не найден." });
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Results.BadRequest(new ErrorResponse { Message = ex.Message });
                    }
                })
            .WithSummary("Удалить штраф")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
        
        return api;
    }
}