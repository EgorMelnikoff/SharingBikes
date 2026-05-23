using sharing_bikes.net.dto.request;
using sharing_bikes.net.model;

namespace sharing_bikes.net.interfaces;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAll();
    Task<User?> GetUserById(Guid id);
    Task<User> CreateUser(CreateUserRequest request);
    Task<bool> DeleteUser(Guid id);
}