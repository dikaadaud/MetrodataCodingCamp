using API.Contracts;
using API.DTOs.Rooms;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public IEnumerable<RoomDto> GetRoom()
    {
        var universities = _roomRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<RoomDto>(); // No universities found
        List<RoomDto> roomDtos = new();
        
        foreach (var room in universities)
        {
            roomDtos.Add((RoomDto) room);
        }
        
        return roomDtos; // Universities found
    }

    public RoomDto? GetRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);
        if (room is null) return null; // Room not found

        return (RoomDto) room; // Universities found
    }

    public RoomDto? CreateRoom(NewRoomDto newRoomDto)
    {
        var createdRoom = _roomRepository.Create(newRoomDto);
        if (createdRoom is null) return null; // Room failed to create

        return (RoomDto) createdRoom; // Room created
    }

    public int UpdateRoom(RoomDto roomDto)
    {
        var getRoom = _roomRepository.GetByGuid(roomDto.Guid);

        if (getRoom is null) return -1; // Room not found
        
        var isUpdate = _roomRepository.Update(roomDto);
        return !isUpdate ? 0 : // Room failed to update
            1;                 // Room updated
    }

    public int DeleteRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);

        if (room is null) return -1; // Room not found

        var isDelete = _roomRepository.Delete(room);
        return !isDelete ? 0 : // Room failed to delete
            1;                 // Room deleted
    }
}
