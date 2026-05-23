using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.database;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.model;

namespace sharing_bikes.net.services;

public class UserService(ShopDbContext db) : IUserService
{
    public async Task<IReadOnlyList<User>> GetAll()
        => await db.Users
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .ToListAsync();

    public async Task<User?> GetUserById(Guid id)
        => await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> CreateUser(CreateUserRequest request)
    {
        ValidateUserFields(request);

        var id = Guid.NewGuid();
        if (await db.Users.AnyAsync(x => x.Id == id))
        {
            throw new InvalidOperationException($"Юзер с идентификатором {id} уже существует.");
        }

        var entity = new User(
            id, request.FullName.Trim(), request.Email.Trim(), request.Phone.Trim()
        );

        db.Users.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }


    public async Task<bool> DeleteUser(Guid id)
    {
        var entity = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        db.Users.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }


    private static void ValidateUserFields(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ArgumentException("ФИО не должно быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Email не должен быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            throw new ArgumentException("Телефон не должен быть пустым.");
        }
    }
}