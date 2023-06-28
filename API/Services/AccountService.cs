using API.Contracts;
using API.Data;
using API.DTOs.AccountRoles;
using API.DTOs.Accounts;
using API.DTOs.Educations;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Handlers;

namespace API.Services;

public class AccountService
{
    private readonly BookingManagementDbContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountService(IAccountRepository accountRepository,
                          IUniversityRepository universityRepository,
                          IEmployeeRepository employeeRepository,
                          IEducationRepository educationRepository,
                          IAccountRoleRepository accountRoleRepository,
                          BookingManagementDbContext context)
    {
        _accountRepository = accountRepository;
        _universityRepository = universityRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _accountRoleRepository = accountRoleRepository;
        _context = context;
    }

    public bool RegisterAccount(RegisterDto registerVM)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var university = _universityRepository.Create(new NewUniversityDto {
                Code = registerVM.UniversityCode,
                Name = registerVM.UniversityName
            });

            Employee employeeData = new NewEmployeeDto {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                BirthDate = registerVM.BirthDate,
                Gender = registerVM.Gender,
                HiringDate = registerVM.HiringDate,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber
            };
            employeeData.Nik = GenerateHandler.Nik(_employeeRepository.GetLastEmpoyeeNik());
            
            var employee = _employeeRepository.Create(employeeData);

            var education = _educationRepository.Create(new EducationDto {
                Guid = employee.Guid,
                Major = registerVM.Major,
                Degree = registerVM.Degree,
                Gpa = registerVM.Gpa,
                UniversityGuid = university.Guid,
            });

            var account = _accountRepository.Create(new AccountDto {
                Guid = employee.Guid,
                Password = HashingHandler.Hash(registerVM.Password)
            });

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    public IEnumerable<AccountDto> GetAccount()
    {
        var universities = _accountRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<AccountDto>(); // No universities found
        List<AccountDto> accountDtos = new();

        foreach (var account in universities)
        {
            accountDtos.Add((AccountDto)account);
        }

        return accountDtos; // Universities found
    }

    public AccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null) return null; // Account not found

        return (AccountDto)account; // Universities found
    }

    public AccountDto? CreateAccount(AccountDto accountDto)
    {
        var createdAccount = _accountRepository.Create(accountDto);
        if (createdAccount is null) return null; // Account failed to create

        return (AccountDto)createdAccount; // Account created
    }

    public int UpdateAccount(AccountDto accountDto)
    {
        var getAccount = _accountRepository.GetByGuid(accountDto.Guid);

        if (getAccount is null) return -1; // Account not found

        var isUpdate = _accountRepository.Update(accountDto);
        return !isUpdate
            ? 0
            :  // Account failed to update
            1; // Account updated
    }

    public int DeleteAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);

        if (account is null) return -1; // Account not found

        var isDelete = _accountRepository.Delete(account);
        return !isDelete
            ? 0
            :  // Account failed to delete
            1; // Account deleted
    }
}
