using sharing_bikes.net.Enum;

namespace sharing_bikes.net.model;

public class Ride {
    public Guid Id { get; }
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public Decimal TotalCost { get; set; }

    public Ride(Guid id, Guid userId, Guid vehicleId)
    {
        Id = id;
        UserId = userId;
        VehicleId = vehicleId;
        StartTime = DateTime.UtcNow;
        EndTime = null;
        TotalCost = 0;
    }
}