using Microsoft.EntityFrameworkCore;
using sharing_bikes.net.api;
using sharing_bikes.net.database;
using sharing_bikes.net.dto;
using sharing_bikes.net.interfaces;
using sharing_bikes.net.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Сервис аренды самокатов",
        Version = "v1",
        Description = "Демонстрационный API сервиса"
    });
});

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Не найдена строка подключения ConnectionStrings:Postgres.");

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<IMapper, Mapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sharing Bikes API v1");
        options.RoutePrefix = "swagger";
    });
}

var api = app.MapGroup("/api");
api.MapRidesEndpoints();
api.MapVehiclesEndpoints();
api.MapUsersEndpoints();

app.MapGet("/", () => Results.Ok(new
{
    message = "API работает."
}));

await app.RunAsync();
