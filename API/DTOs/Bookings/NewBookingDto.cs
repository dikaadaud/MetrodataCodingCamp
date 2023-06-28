using API.Models;
using API.Utilities;
using API.Utilities.Enums;

namespace API.DTOs.Bookings;

public class NewBookingDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }
    
    public static implicit operator Booking(NewBookingDto newBookingDto)
    {
        return new() {
            Guid = new Guid(),
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = StatusLevel.Requested,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid
        };
    }
    
    public static explicit operator NewBookingDto(Booking booking)
    {
        return new() {
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };
    }
}
