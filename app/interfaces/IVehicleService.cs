using sharing_bikes.net.dto.request;
using sharing_bikes.net.model;

namespace sharing_bikes.net.interfaces;

public interface IVehicleService
{
    Task<IReadOnlyList<Vehicle>> GetAll();
    Task<Vehicle?> GetVehicleById(Guid id);
    
    Task<Vehicle> CreateVehicle(CreateVehicleRequest request);
    
    Task<Vehicle?> PatchVehicle(Guid id, PatchVehicleRequest request);
    
    //patch
    Task<bool> DeleteVehicle(Guid id);
}