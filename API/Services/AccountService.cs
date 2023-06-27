using API.Contracts;
using API.DTOs.Accounts;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    
    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public IEnumerable<AccountDto> GetAccount()
    {
        var universities = _accountRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<AccountDto>(); // No universities found
        List<AccountDto> accountDtos = new();
        
        foreach (var account in universities)
        {
            accountDtos.Add((AccountDto) account);
        }
        
        return accountDtos; // Universities found
    }

    public AccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null) return null; // Account not found

        return (AccountDto) account; // Universities found
    }

    public AccountDto? CreateAccount(AccountDto accountDto)
    {
        var createdAccount = _accountRepository.Create(accountDto);
        if (createdAccount is null) return null; // Account failed to create

        return (AccountDto) createdAccount; // Account created
    }

    public int UpdateAccount(AccountDto accountDto)
    {
        var getAccount = _accountRepository.GetByGuid(accountDto.Guid);

        if (getAccount is null) return -1; // Account not found
        
        var isUpdate = _accountRepository.Update(accountDto);
        return !isUpdate ? 0 : // Account failed to update
            1;                 // Account updated
    }

    public int DeleteAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);

        if (account is null) return -1; // Account not found

        var isDelete = _accountRepository.Delete(account);
        return !isDelete ? 0 : // Account failed to delete
            1;                 // Account deleted
    }
}
