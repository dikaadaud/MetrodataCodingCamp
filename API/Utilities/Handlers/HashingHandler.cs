namespace API.Utilities.Handlers;

public class HashingHandler
{
    private static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12); // default is 12 rounds
    }

    public static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GenerateSalt());
    }

    public static bool Validate(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}
