using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.model;

namespace sharing_bikes.net.interfaces;

public interface IRideService {
    Task<IReadOnlyList<Ride>> GetAll();
    Task<Ride?> GetRideById(Guid id);
    Task<Ride?> GetRideByUserId(Guid userId);
    Task<StartRideResponse> StartRide(StartRideRequest request);
    Task<Ride> EndRide(EndRideRequest request);
}