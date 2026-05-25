using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.database;
using sharing_bikes.net.dto.request;
using sharing_bikes.net.Enum;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.model;

namespace sharing_bikes.net.services;

public class FineService(SharingDbContext db) : IFineService
{
    public async Task<IReadOnlyList<Fine>> GetAll()
        => await db.Fines
            .AsNoTracking()
            .ToListAsync();

    public async Task<Fine?> GetFineById(Guid id)
        => await db.Fines
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Fine>> GetActiveFineByUserId(Guid userId)
        => await db.Fines
            .Where(x => x.UserId == userId && x.Status == FineStatus.Active)
            .ToListAsync();

    public async Task<Fine> CreateFine(CreateFineRequest request)
    {
        var id = Guid.NewGuid();
        if (await db.Fines.AnyAsync(x => x.Id == id))
        {
            throw new InvalidOperationException($"Штраф с идентификатором {id} уже существует.");
        }

        var entity = new Fine(
            id, request.UserId, request.RideId, request.Amount, request.Reason
        );

        db.Fines.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<Fine?> PatchFine(Guid id, PatchFineRequest request)
    {
        var entity = await db.Fines.FirstOrDefaultAsync(x => x.Id == id);

        entity?.Amount = request.Amount ?? entity.Amount;
        entity?.Status = request.FineStatus ?? entity.Status;

        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteFIne(Guid id)
    {
        var entity = await db.Fines.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        db.Fines.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}