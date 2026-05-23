namespace sharing_bikes.net.dto.request;

public record EndRideRequest
{
    public Guid RideId { get; init; }
}