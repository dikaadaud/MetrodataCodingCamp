using API.Contracts;
using API.DTOs.Roles;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public IEnumerable<RoleDto> GetRole()
    {
        var universities = _roleRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<RoleDto>(); // No universities found
        List<RoleDto> roleDtos = new();
        
        foreach (var role in universities)
        {
            roleDtos.Add((RoleDto) role);
        }
        
        return roleDtos; // Universities found
    }

    public RoleDto? GetRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);
        if (role is null) return null; // Role not found

        return (RoleDto) role; // Universities found
    }

    public RoleDto? CreateRole(NewRoleDto newRoleDto)
    {
        var createdRole = _roleRepository.Create(newRoleDto);
        if (createdRole is null) return null; // Role failed to create

        return (RoleDto) createdRole; // Role created
    }

    public int UpdateRole(RoleDto roleDto)
    {
        var getRole = _roleRepository.GetByGuid(roleDto.Guid);

        if (getRole is null) return -1; // Role not found
        
        var isUpdate = _roleRepository.Update(roleDto);
        return !isUpdate ? 0 : // Role failed to update
            1;                 // Role updated
    }

    public int DeleteRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);

        if (role is null) return -1; // Role not found

        var isDelete = _roleRepository.Delete(role);
        return !isDelete ? 0 : // Role failed to delete
            1;                 // Role deleted
    }
}
