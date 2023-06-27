using System.Net;
using API.DTOs.Bookings;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
        private readonly BookingService _service;

    public BookingController(BookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetBooking();

        if (!entities.Any())
            return NotFound(new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<IEnumerable<BookingDto>> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var booking = _service.GetBooking(guid);
        if (booking is null)
            return NotFound(new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });

        return Ok(new ResponseHandler<BookingDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = booking
        });
    }

    [HttpPost]
    public IActionResult Create(NewBookingDto newBookingDto)
    {
        var createdBooking = _service.CreateBooking(newBookingDto);
        if (createdBooking is null)
            return BadRequest(new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });

        return Ok(new ResponseHandler<BookingDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = createdBooking
        });
    }

    [HttpPut]
    public IActionResult Update(BookingDto updateBookingDto)
    {
        var update = _service.UpdateBooking(updateBookingDto);
        if (update is -1)
            return NotFound(new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        
        if (update is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        
        return Ok(new ResponseHandler<BookingDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteBooking(guid);

        if (delete is -1)
            return NotFound(new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        
        if (delete is 0)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<BookingDto> {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<BookingDto> {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
