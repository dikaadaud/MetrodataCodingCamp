using API.Contracts;
using API.DTOs.Bookings;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public IEnumerable<BookingDto> GetBooking()
    {
        var universities = _bookingRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<BookingDto>(); // No universities found
        List<BookingDto> bookingDtos = new();
        
        foreach (var booking in universities)
        {
            bookingDtos.Add((BookingDto) booking);
        }
        
        return bookingDtos; // Universities found
    }

    public BookingDto? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null) return null; // Booking not found

        return (BookingDto) booking; // Universities found
    }

    public BookingDto? CreateBooking(NewBookingDto newBookingDto)
    {
        var createdBooking = _bookingRepository.Create(newBookingDto);
        if (createdBooking is null) return null; // Booking failed to create

        return (BookingDto) createdBooking; // Booking created
    }

    public int UpdateBooking(BookingDto bookingDto)
    {
        var getBooking = _bookingRepository.GetByGuid(bookingDto.Guid);

        if (getBooking is null) return -1; // Booking not found
        
        var isUpdate = _bookingRepository.Update(bookingDto);
        return !isUpdate ? 0 : // Booking failed to update
            1;                 // Booking updated
    }

    public int DeleteBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);

        if (booking is null) return -1; // Booking not found

        var isDelete = _bookingRepository.Delete(booking);
        return !isDelete ? 0 : // Booking failed to delete
            1;                 // Booking deleted
    }
}
