using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.response;

public record VehicleResponse
{
    public Guid Id { get; init; }
    public string Model { get; init; }
    public string Number { get; init; }
    public VehicleStatus Status { get; init; }
}