using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Auth
{
    public static class CustomerSecurityExtensions
    {
        public static bool VerifyPassword(this Customer customer, string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: customer.PasswordSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return customer.PasswordHash.Equals(hashed);
        }

        public static void AssignPassword(this Customer customer, string password)
        {
            // Generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            customer.PasswordSalt = salt;
            customer.PasswordHash = hashed;
        }
    }
}
