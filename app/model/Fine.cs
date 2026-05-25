using sharing_bikes.net.Enum;

namespace sharing_bikes.net.model;

public class Fine {
    public Guid Id { get; }
    public Guid UserId { get; set; }
    public Guid RideId { get; set; }

    public Decimal Amount { get; set; }
    public string Reason { get; set; }
    
    public FineStatus Status { get; set; }

    public Fine(Guid id, Guid userId, Guid rideId, Decimal amount, string reason)
    {
        Id = id;
        UserId = userId;
        RideId = rideId;
        Status = FineStatus.Active;
        Amount = amount;
        Reason = reason;
    }
}