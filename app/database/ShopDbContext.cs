namespace sharing_bikes.net.database;

using Microsoft.EntityFrameworkCore;
using model;


public class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Ride> Rides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(300).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(20);
        });
        
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Model).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Number).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Status).IsRequired();
        });
        
        modelBuilder.Entity<Ride>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TotalCost).HasPrecision(18, 2);
            
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}