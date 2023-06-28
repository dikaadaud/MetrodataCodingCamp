using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities;

public class UpdateUniversityValidator : AbstractValidator<UniversityDto>
{
    public UpdateUniversityValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();
        
        RuleFor(p => p.Code)
           .NotEmpty();

        RuleFor(p => p.Name)
           .NotEmpty();
    }
}
