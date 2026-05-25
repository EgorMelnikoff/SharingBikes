using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.database;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.dto.response;
using sharing_bikes.net.Enum;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.model;

namespace sharing_bikes.net.services;

public class RideService(ShopDbContext db, IVehicleService vehicleService) : IRideService
{
    public async Task<IReadOnlyList<Ride>> GetAll()
        => await db.Rides
            .AsNoTracking()
            .OrderBy(x => x.StartTime)
            .ToListAsync();

    public async Task<Ride?> GetRideById(Guid id)
    {
        Ride? ride = await db.Rides
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ride is null)
        {
            return null;
        }

        ride.TotalCost = GetCurrentCost(ride.StartTime, ride.EndTime);
        return ride;
    }
    
    public async Task<Ride?> GetRideByUserId(Guid userId)
    {
        Ride? ride = await db.Rides
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.EndTime == null);
        
        ride?.TotalCost = GetCurrentCost(ride.StartTime, ride.EndTime);
        return ride;
    }

    public async Task<StartRideResponse> StartRide(StartRideRequest request)
    {
        Ride? ride = await GetRideByUserId(request.UserId);
        
        if (ride is not null)
        {
            throw new InvalidOperationException($"Невозможно начать новую поездку без завершения старой.");
        }
        
        Vehicle? vehicle = await vehicleService.GetVehicleById(request.VehicleId);

        if (vehicle is null)
        {
            throw new InvalidOperationException($"Самоката не существует.");
        }

        if (vehicle.Status != VehicleStatus.Available)
        {
            throw new InvalidOperationException($"Самокат недоступен.");
        }
        

        var id = Guid.NewGuid();
        var entity = new Ride(
            id, request.UserId, request.VehicleId, DateTime.Now, null
        );

        db.Rides.Add(entity);

        vehicleService.PatchVehicle(
            request.VehicleId,
            new PatchVehicleRequest(null, VehicleStatus.Unavailable)
        );

        await db.SaveChangesAsync();
        return new StartRideResponse(entity);
    }


    public async Task<Ride> EndRide(EndRideRequest request)
    {
        Ride? ride = await GetRideById(request.RideId);

        if (ride is null)
        {
            throw new InvalidOperationException($"Поездки не существует.");
        }
        
        Vehicle? vehicle = await vehicleService.GetVehicleById(ride.VehicleId);

        if (vehicle is null)
        {
            throw new InvalidOperationException($"Самоката не существует.");
        }
        
        vehicle.Status = VehicleStatus.Available;
        ride.EndTime = DateTime.Now;
        ride.TotalCost = GetCurrentCost(ride.StartTime, ride.EndTime);
        await db.SaveChangesAsync();
        
        return ride;
    }

    private decimal GetCurrentCost(DateTime start, DateTime? end)
    {
        decimal pricePerMinute = 9m;

        double minutes = end.HasValue
            ? (end.Value - start).TotalMinutes
            : (DateTime.Now - start).TotalMinutes;

        if (minutes < 0)
            minutes = 0;

        return (decimal)minutes * pricePerMinute;
    }
}