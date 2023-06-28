using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }
    public bool IsDuplicateValue(string value)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.Email.Contains(value) || e.PhoneNumber.Contains(value)) is null;
    }

    public string? GetLastEmpoyeeNik()
    {
        return _context.Set<Employee>().ToList().Select(e => e.Nik).LastOrDefault();
    }
}
