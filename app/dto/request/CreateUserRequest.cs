namespace sharing_bikes.net.dto.request;

public record CreateUserRequest
{
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
}