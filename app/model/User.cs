namespace sharing_bikes.net.model;

public class User {
    public Guid Id { get; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    public User(
        Guid id, 
        string fullName,
        string email,
        string phone
        )
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Phone = phone;
    }
}