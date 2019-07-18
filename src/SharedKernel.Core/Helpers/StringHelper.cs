using System.Text.RegularExpressions;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Helper for string operations
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Apply regex to let only numbers
        /// </summary>
        /// <param name="text">text to replace</param>
        /// <returns>string with only numbers</returns>
        public static string OnlyNumbers(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Regex.Replace(text, Constants.RegexOnlyNumbers, string.Empty);
        }

        /// <summary>
        /// Apply regex to let only text
        /// </summary>
        /// <param name="text">text to replace</param>
        /// <returns>string with only letters</returns>
        public static string OnlyLetters(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Regex.Replace(text, Constants.RegexOnlyLetters, string.Empty);
        }
    }
}
