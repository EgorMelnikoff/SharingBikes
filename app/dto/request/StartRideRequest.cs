namespace sharing_bikes.net.dto.request;

public record StartRideRequest
{
    public Guid UserId { get; init; }
    public Guid VehicleId { get; init; }
}