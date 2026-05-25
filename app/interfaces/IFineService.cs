using sharing_bikes.net.dto.request;
using sharing_bikes.net.model;

namespace sharing_bikes.net.interfaces;

public interface IFineService
{
    Task<IReadOnlyList<Fine>> GetAll();
    Task<Fine?> GetFineById(Guid id);
    Task<IReadOnlyList<Fine>> GetActiveFineByUserId(Guid userId);
    
    Task<Fine?> PatchFine(Guid id, PatchFineRequest request);
    Task<Fine> CreateFine(CreateFineRequest request);
    Task<bool> DeleteFIne(Guid id);
}