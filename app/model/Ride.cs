using sharing_bikes.net.Enum;

namespace sharing_bikes.net.model;

public class Ride {
    public Guid Id { get; }
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public Decimal TotalCost { get; set; }

    public Ride(Guid id, Guid userId, Guid vehicleId, DateTime startTime, DateTime? endTime)
    {
        Id = id;
        UserId = userId;
        VehicleId = vehicleId;
        StartTime = startTime;
        EndTime = endTime;
        TotalCost = 0;
    }
}