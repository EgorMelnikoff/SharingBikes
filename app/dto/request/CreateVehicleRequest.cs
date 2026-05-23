using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.request;

public record CreateVehicleRequest
{
    public string Model { get; init; }
    public string Number { get; init; }
    public VehicleStatus Status { get; init; }
}