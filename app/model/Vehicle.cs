using sharing_bikes.net.Enum;

namespace sharing_bikes.net.model;

public class Vehicle {
    public Guid Id { get; set; }
    public string Model { get; set; }
    public string Number { get; set; }
    public VehicleStatus Status { get; set; }

    public Vehicle(Guid id, string model, string number)
    {
        Id = id;
        Model = model;
        Number = number;
        Status = VehicleStatus.Available;
    }
}