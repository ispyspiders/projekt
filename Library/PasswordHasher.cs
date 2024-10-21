// Lösenords hashare enligt tutorial på : https://yarkul.com/hash-salt-store-password-in-csharp/

using System.Security.Cryptography;

namespace ptApp
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char SaltDelimiter = ';';

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return string.Join(SaltDelimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Validate(string passwordHash, string password)
        {
            var passwordElements = passwordHash.Split(SaltDelimiter);
            var salt = Convert.FromBase64String(passwordElements[0]);
            var hash = Convert.FromBase64String(passwordElements[1]);
            var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }

    }
}