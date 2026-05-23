using sharing_bikes.net.dto.response;
using sharing_bikes.net.model;

namespace sharing_bikes.net.dto;

public class Mapper : IMapper
{
    public VehicleResponse Map(Vehicle vehicle) => new()
    {
        Id = vehicle.Id,
        Model = vehicle.Model,
        Number = vehicle.Number,
        Status = vehicle.Status
    };

    public UserResponse Map(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Phone = user.Phone
    };

    public RideResponse Map(Ride ride) => new()
    {
        Id = ride.Id,
        UserId = ride.UserId,
        VehicleId = ride.VehicleId,
        StartTime = ride.StartTime,
        EndTime = ride.EndTime,
        TotalCost = ride.TotalCost
    };
}