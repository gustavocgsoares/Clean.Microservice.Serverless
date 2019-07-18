using System.Security.Cryptography;
using System.Text;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Help to crypt/decrypt values.
    /// </summary>
    public static class CryptHelper
    {
        /// <summary>
        /// Create SHA512 Hash.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>SHA512 hash based on input data</returns>
        public static string CreateSha512(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var sha512 = SHA512Managed.Create();

            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha512.ComputeHash(bytes);

            return GetStringFromSHA512Hash(hash);
        }

        private static string GetStringFromSHA512Hash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
