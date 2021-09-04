using System.Security.Cryptography;

namespace AuthorizationService.Extensions
{
    public static class StringExtensions
    {
        private const int NUMBER_OF_ROUNDS = 1000;
        public static byte[] ToPasswordHash(this string self, byte[] salt)
        {
            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(self, salt, NUMBER_OF_ROUNDS);
            return rfc2898DeriveBytes.GetBytes(20);
        }
    }
}
