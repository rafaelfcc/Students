using Escola.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class PasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(User user, string password, string hashedPassword)
    {
        return _hasher.VerifyHashedPassword(user, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}
