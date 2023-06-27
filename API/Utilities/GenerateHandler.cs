using API.Contracts;

namespace API.Utilities;

public class GenerateHandler
{
    private static IEmployeeRepository _employeeRepository;

    public GenerateHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public static string Nik()
    {
        var getLastNik = _employeeRepository.GetAll()
                                            .Select(employee => employee.Nik)
                                            .LastOrDefault();
        
        if (getLastNik is null) return "111111"; // First employee

        var lastNik = Convert.ToInt32(getLastNik) + 1;
        return lastNik.ToString();
    }
}
