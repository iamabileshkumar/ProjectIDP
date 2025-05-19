using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace SubscriptionMonitoring.WebAPI.Services
{
    public class PasswordService
    {
        public bool VerifyPassword(string storedHash, string providedPassword)
        {
            // Extract the salt and hashed password from the stored value
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHashedPassword = parts[1];

            // Hash the provided password with the same salt
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare the hashed passwords
            return hashed == storedHashedPassword;
        }

        public string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Combine the salt and hashed password for storage
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }
    }

}
