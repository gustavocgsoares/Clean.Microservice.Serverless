using System;
using System.Text;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Jwt Helper
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Parse JWT token as destination object.
        /// </summary>
        /// <typeparam name="TDestination">Destination object.</typeparam>
        /// <param name="destination"></param>
        /// <param name="token">Token JWT</param>
        /// <returns>TDestination</returns>
        public static TDestination ParseTokenAsObject<TDestination>(this TDestination destination, string token)
        {
            var decodedToken = DecodeToken(token);
            var payload = DecodedTokenAsPayload<TDestination>(decodedToken);

            return payload;
        }

        private static TDestination DecodedTokenAsPayload<TDestination>(byte[] payload)
        {
            var convertedPayload = Encoding.UTF8.GetString(payload);
            var result = JsonConvert.DeserializeObject<TDestination>(convertedPayload);

            return result;
        }

        /// <summary>
        /// Decodes jwt token
        /// </summary>
        /// <param name="token">JwtToken</param>
        /// <returns>base64 converted at byte</returns>
        private static byte[] DecodeToken(string token)
        {
            var output = token.Split('.')[1];
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');

            switch (output.Length % 4)
            {
                case 0: break;
                case 2: output += "=="; break;
                case 3: output += "="; break;
                default: throw new ArgumentOutOfRangeException(nameof(token), "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output);

            return converted;
        }
    }
}
