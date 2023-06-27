using API.Models;

namespace API.DTOs.Accounts;

public class AccountDto
{
    public Guid Guid { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public int Otp { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    public static implicit operator Account(AccountDto accountDto)
    {
        return new() {
            Guid = accountDto.Guid,
            Password = accountDto.Password,
            IsDeleted = accountDto.IsDeleted,
            OTP = accountDto.Otp,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime
        };
    }
    
    public static explicit operator AccountDto(Account account)
    {
        return new() {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            Otp = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }
}
