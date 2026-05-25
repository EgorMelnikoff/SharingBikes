using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.response;

public class FineResponse {
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid RideId { get; init; }

    public Decimal Amount { get; init; }
    public string Reason { get; init; }
}