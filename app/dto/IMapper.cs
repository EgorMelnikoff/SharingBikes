using sharing_bikes.net.dto.response;
using sharing_bikes.net.model;

namespace sharing_bikes.net.dto;

public interface IMapper
{
    VehicleResponse Map(Vehicle vehicle);
    
    UserResponse Map(User user, Ride? ride);
    
    RideResponse Map(Ride ride);
}
