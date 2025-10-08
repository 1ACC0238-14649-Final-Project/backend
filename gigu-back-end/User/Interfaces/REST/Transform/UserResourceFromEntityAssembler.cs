// UserResourceFromEntityAssembler.cs
using gigu_back_end.User.Domain.Models.Entities;
using gigu_back_end.User.Interfaces.REST.Resources;

namespace gigu_back_end.User.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(Domain.Models.Entities.User user) =>
        new(user.Id, user.Name, user.Lastname, user.Email, user.Role, user.Image);
}