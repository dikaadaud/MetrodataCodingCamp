using API.Contracts;
using API.DTOs.Educations;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }
    
    public IEnumerable<EducationDto> GetEducation()
    {
        var universities = _educationRepository.GetAll().ToList();
        if (!universities.Any()) return Enumerable.Empty<EducationDto>(); // No universities found
        List<EducationDto> educationDtos = new();
        
        foreach (var education in universities)
        {
            educationDtos.Add((EducationDto) education);
        }
        
        return educationDtos; // Universities found
    }

    public EducationDto? GetEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null) return null; // Education not found

        return (EducationDto) education; // Universities found
    }

    public EducationDto? CreateEducation(EducationDto educationDto)
    {
        var createdEducation = _educationRepository.Create(educationDto);
        if (createdEducation is null) return null; // Education failed to create

        return (EducationDto) createdEducation; // Education created
    }

    public int UpdateEducation(EducationDto educationDto)
    {
        var getEducation = _educationRepository.GetByGuid(educationDto.Guid);

        if (getEducation is null) return -1; // Education not found
        
        var isUpdate = _educationRepository.Update(educationDto);
        return !isUpdate ? 0 : // Education failed to update
            1;                 // Education updated
    }

    public int DeleteEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);

        if (education is null) return -1; // Education not found

        var isDelete = _educationRepository.Delete(education);
        return !isDelete ? 0 : // Education failed to delete
            1;                 // Education deleted
    }
}
