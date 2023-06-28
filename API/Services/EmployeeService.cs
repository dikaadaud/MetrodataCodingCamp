using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handlers;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public IEnumerable<EmployeeDto> GetEmployee()
    {
        var universities = _employeeRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<EmployeeDto>(); // No universities found
        List<EmployeeDto> employeeDtos = new();
        
        foreach (var employee in universities)
        {
            employeeDtos.Add((EmployeeDto) employee);
        }
        
        return employeeDtos; // Universities found
    }

    public EmployeeDto? GetEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null) return null; // Employee not found

        return (EmployeeDto) employee; // Universities found
    }

    public EmployeeDto? CreateEmployee(NewEmployeeDto newEmployeeDto)
    {
        Employee employee = newEmployeeDto;
        employee.Nik = GenerateHandler.Nik(_employeeRepository.GetLastEmpoyeeNik());
        
        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null) return null; // Employee failed to create

        return (EmployeeDto) createdEmployee; // Employee created
    }

    public int UpdateEmployee(EmployeeDto employeeDto)
    {
        var getEmployee = _employeeRepository.GetByGuid(employeeDto.Guid);

        if (getEmployee is null) return -1; // Employee not found
        
        var isUpdate = _employeeRepository.Update(employeeDto);
        return !isUpdate ? 0 : // Employee failed to update
            1;                 // Employee updated
    }

    public int DeleteEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);

        if (employee is null) return -1; // Employee not found

        var isDelete = _employeeRepository.Delete(employee);
        return !isDelete ? 0 : // Employee failed to delete
            1;                 // Employee deleted
    }
}
