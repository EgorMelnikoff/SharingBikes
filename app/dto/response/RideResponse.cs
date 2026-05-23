namespace sharing_bikes.net.dto.response;

public record RideResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid VehicleId { get; init; }

    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    
    public Decimal TotalCost { get; init; }
};