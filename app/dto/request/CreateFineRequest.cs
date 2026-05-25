using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.request;

public class CreateFineRequest {
    public Guid UserId { get; init; }
    public Guid RideId { get; init; }

    public Decimal Amount { get; init; }
    public string Reason { get; init; }
}