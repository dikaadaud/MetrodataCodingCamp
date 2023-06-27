using API.Models;

namespace API.DTOs.Universities;

public class NewUniversityDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    
    public static implicit operator University(NewUniversityDto newUniversityDto)
    {
        return new() {
            Code = newUniversityDto.Code,
            Name = newUniversityDto.Name
        };
    }
    
    public static explicit operator NewUniversityDto(University university)
    {
        return new() {
            Code = university.Code,
            Name = university.Name
        };
    }
}
