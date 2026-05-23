using sharing_bikes.net.model;

namespace sharing_bikes.net.dto.response;

public record StartRideResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid VehicleId { get; init; }

    public DateTime StartTime { get; init; }

    public StartRideResponse(Ride ride)
    {
        Id = ride.Id;
        UserId = ride.UserId;
        VehicleId = ride.VehicleId;
        StartTime = ride.StartTime;
    }
}