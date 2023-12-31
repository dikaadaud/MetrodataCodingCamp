﻿using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    bool IsDuplicateValue(string value);
    string? GetLastEmpoyeeNik();
    Employee? GetEmployeeByEmail(string email);
}
