using System.Net;
using API.DTOs.Educations;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EducationController : ControllerBase
{
        private readonly EducationService _service;

    public EducationController(EducationService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetEducation();

        if (!entities.Any())
            return NotFound(new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<IEnumerable<EducationDto>> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var education = _service.GetEducation(guid);
        if (education is null)
            return NotFound(new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<EducationDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = education
        });
    }

    [HttpPost]
    public IActionResult Create(EducationDto educationDto)
    {
        var createdEducation = _service.CreateEducation(educationDto);
        if (createdEducation is null)
            return BadRequest(new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });

        return Ok(new ResponseHandler<EducationDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = createdEducation
        });
    }

    [HttpPut]
    public IActionResult Update(EducationDto updateEducationDto)
    {
        var update = _service.UpdateEducation(updateEducationDto);
        if (update is -1)
            return NotFound(new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        
        if (update is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        
        return Ok(new ResponseHandler<EducationDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteEducation(guid);

        if (delete is -1)
            return NotFound(new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        
        if (delete is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<EducationDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<EducationDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
