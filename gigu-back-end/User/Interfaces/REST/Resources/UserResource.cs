// UserResource.cs
namespace gigu_back_end.User.Interfaces.REST.Resources;

public record UserResource(int Id, string Name, string LastName, string Email, string Role, string Image);