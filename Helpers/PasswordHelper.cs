using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AnimeCollection.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

        return $"{Convert.ToBase64String(salt)}.{hash}";
    }

    public static bool VerifyPassword(
        string password,
        string storedHash)
    {
        var parts = storedHash.Split('.');

        byte[] salt = Convert.FromBase64String(parts[0]);

        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

        return hash == parts[1];
    }
}