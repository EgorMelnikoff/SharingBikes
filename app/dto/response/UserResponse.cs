namespace sharing_bikes.net.dto.response;

public record UserResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
}