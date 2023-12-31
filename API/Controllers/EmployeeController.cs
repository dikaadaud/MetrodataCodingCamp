﻿using System.Net;
using API.DTOs.Employees;
using API.Services;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _service;

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetEmployee();

        if (!entities.Any())
            return NotFound(new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<IEnumerable<EmployeeDto>> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _service.GetEmployee(guid);
        if (employee is null)
            return NotFound(new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<EmployeeDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = employee
        });
    }

    [HttpPost]
    public IActionResult Create(NewEmployeeDto newEmployeeDto)
    {
        var createdEmployee = _service.CreateEmployee(newEmployeeDto);
        if (createdEmployee is null)
            return BadRequest(new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });

        return Ok(new ResponseHandler<EmployeeDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = createdEmployee
        });
    }

    [HttpPut]
    public IActionResult Update(EmployeeDto updateEmployeeDto)
    {
        var update = _service.UpdateEmployee(updateEmployeeDto);
        if (update is -1)
            return NotFound(new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });

        if (update is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<EmployeeDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteEmployee(guid);

        if (delete is -1)
            return NotFound(new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });

        if (delete is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<EmployeeDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<EmployeeDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
