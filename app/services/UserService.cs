using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.database;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.model;

namespace sharing_bikes.net.services;

public class UserService(SharingDbContext db) : IUserService
{
    public async Task<IReadOnlyList<User>> GetAll()
        => await db.Users
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .ToListAsync();

    public async Task<User?> GetUserById(Guid id)
        => await db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> CreateUser(CreateUserRequest request)
    {
        ValidateUserFields(request);
        
        var email = request.Email.Trim().ToLower();
        var phone = request.Phone.Trim();

        var isEmailTaken = await db.Users.AnyAsync(x => x.Email.ToLower() == email);
        if (isEmailTaken)
        {
            throw new InvalidOperationException($"Пользователь с email '{request.Email}' уже зарегистрирован.");
        }

        var isPhoneTaken = await db.Users.AnyAsync(x => x.Phone == phone);
        if (isPhoneTaken)
        {
            throw new InvalidOperationException($"Пользователь с телефоном '{request.Phone}' уже зарегистрирован.");
        }
        
        var entity = new User(
            Guid.NewGuid(), request.FullName.Trim(), request.Email.Trim(), request.Phone.Trim()
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

    
    private void ValidateUserFields(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Имя не может быть пустым.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email не может быть пустым.");

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new ArgumentException("Телефон не может быть пустым.");
    }

}