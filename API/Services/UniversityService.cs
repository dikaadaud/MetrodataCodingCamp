using API.Contracts;
using API.DTOs.Universities;
using API.Models;

namespace API.Services;

public class UniversityService
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public IEnumerable<UniversityDto> GetUniversity()
    {
        var universities = _universityRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<UniversityDto>(); // No universities found
        List<UniversityDto> universityDtos = new();
        
        foreach (var university in universities)
        {
            universityDtos.Add((UniversityDto) university);
        }
        
        return universityDtos; // Universities found
    }

    public UniversityDto? GetUniversity(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);
        if (university is null) return null; // University not found

        return (UniversityDto) university; // Universities found
    }

    public UniversityDto? CreateUniversity(NewUniversityDto newUniversityDto)
    {
        var createdUniversity = _universityRepository.Create(newUniversityDto);
        if (createdUniversity is null) return null; // University failed to create

        return (UniversityDto) createdUniversity; // University created
    }

    public int UpdateUniversity(UniversityDto universityDto)
    {
        var getUniversity = _universityRepository.GetByGuid(universityDto.Guid);

        if (getUniversity is null) return -1; // University not found
        
        var isUpdate = _universityRepository.Update(universityDto);
        return !isUpdate ? 0 : // University failed to update
            1; // University updated
    }

    public int DeleteUniversity(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);

        if (university is null) return -1; // University not found

        var isDelete = _universityRepository.Delete(university);
        return !isDelete ? 0 : // University failed to delete
            1; // University deleted
    }
}
