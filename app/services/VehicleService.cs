using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.database;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.model;

namespace sharing_bikes.net.services;

public class VehicleService(SharingDbContext db) : IVehicleService
{
    public async Task<IReadOnlyList<Vehicle>> GetAll()
        => await db.Vehicles
            .AsNoTracking()
            .OrderBy(x => x.Number)
            .ToListAsync();

    public async Task<Vehicle?> GetVehicleById(Guid id)
        => await db.Vehicles
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Vehicle> CreateVehicle(CreateVehicleRequest request)
    {
        ValidateVehicleFields(request.Model, request.Number);

        var id = Guid.NewGuid();
        if (await db.Vehicles.AnyAsync(x => x.Id == id))
        {
            throw new InvalidOperationException($"Самокат с идентификатором {id} уже существует.");
        }

        var entity = new Vehicle(
            id, request.Model.Trim(), request.Number.Trim()
        );

        db.Vehicles.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<Vehicle?> PatchVehicle(Guid id, PatchVehicleRequest request)
    {
        var entity = await db.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
       
        entity?.Number = request.Number ?? entity.Number;
        entity?.Status = request.Status ?? entity.Status;
        
        ValidateNumber(entity?.Number);
        
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteVehicle(Guid id)
    {
        var entity = await db.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        db.Vehicles.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
    
    private static void ValidateVehicleFields(string model, string number)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("Модель не должна быть пустой.");
        }

        ValidateNumber(number);
    }
    
    private static void ValidateNumber(string? number)
    {
        if (number is not null && number.Length != 6)
        {
            throw new ArgumentException("Номер должен состоять из 6 символов.");
        }
    }
}