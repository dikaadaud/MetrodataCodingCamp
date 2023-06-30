using API.Models;

namespace API.Contracts;

public interface IUniversityRepository : IGeneralRepository<University>
{
    University? CreateWithDuplicateCheck(University university);
}
