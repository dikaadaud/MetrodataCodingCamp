using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingManagementDbContext context) : base(context) { }
    
    public University? CreateWithDuplicateCheck(University university)
    {
        var getUniversity = _context.Universities.FirstOrDefault(u => u.Name == university.Name && u.Code == university.Code);
        
        if (getUniversity != null)
        {
            return getUniversity;
        }
        
        return Create(university);
    }
}
