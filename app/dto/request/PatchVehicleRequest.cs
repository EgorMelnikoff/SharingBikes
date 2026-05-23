using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.request;

public record PatchVehicleRequest
{
    public string? Number { get; init; }
    public VehicleStatus? Status { get; init; }

    public PatchVehicleRequest(string? number, VehicleStatus? status)
    {
        Number = number;
        Status = status;
    }
}