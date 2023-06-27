using API.Contracts;
using API.DTOs.AccountRoles;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleService(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }
    
    public IEnumerable<AccountRoleDto> GetAccountRole()
    {
        var universities = _accountRoleRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<AccountRoleDto>(); // No universities found
        List<AccountRoleDto> accountRoleDtos = new();
        
        foreach (var accountRole in universities)
        {
            accountRoleDtos.Add((AccountRoleDto) accountRole);
        }
        
        return accountRoleDtos; // Universities found
    }

    public AccountRoleDto? GetAccountRole(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null) return null; // AccountRole not found

        return (AccountRoleDto) accountRole; // Universities found
    }

    public AccountRoleDto? CreateAccountRole(AccountRoleDto accountRoleDto)
    {
        var createdAccountRole = _accountRoleRepository.Create(accountRoleDto);
        if (createdAccountRole is null) return null; // AccountRole failed to create

        return (AccountRoleDto) createdAccountRole; // AccountRole created
    }

    public int UpdateAccountRole(AccountRoleDto accountRoleDto)
    {
        var getAccountRole = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);

        if (getAccountRole is null) return -1; // AccountRole not found
        
        var isUpdate = _accountRoleRepository.Update(accountRoleDto);
        return !isUpdate ? 0 : // AccountRole failed to update
            1;                 // AccountRole updated
    }

    public int DeleteAccountRole(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);

        if (accountRole is null) return -1; // AccountRole not found

        var isDelete = _accountRoleRepository.Delete(accountRole);
        return !isDelete ? 0 : // AccountRole failed to delete
            1;                 // AccountRole deleted
    }
}
