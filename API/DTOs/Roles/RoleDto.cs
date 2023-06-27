using API.Models;

namespace API.DTOs.Roles;

public class RoleDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    
    public static implicit operator Role(RoleDto roleDto)
    {
        return new() {
            Guid = roleDto.Guid,
            Name = roleDto.Name
        };
    }
    
    public static explicit operator RoleDto(Role role)
    {
        return new() {
            Guid = role.Guid,
            Name = role.Name
        };
    }
}
