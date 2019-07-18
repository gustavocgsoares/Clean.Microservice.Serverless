using System.Text;
using Microsoft.Azure.EventHubs;
using Clean.Microservice.Serverless.SharedKernel.Core.Serializers;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Helper to work with Json parser
    /// </summary>
    public static class ParserJsonHelper
    {
        /// <summary>
        /// Convert Azure Event Hub Event data to TDestination
        /// </summary>
        /// <typeparam name="TDestination">Destination object</typeparam>
        /// <param name="eventData">eventa data</param>
        /// <param name="usePrivateCamelCaseSerializer">Case not informed will be used JsonSerializerSettings().GetJsonSerializerSettingsWithPrivateCamelCaseSerializer()</param>
        /// <returns>object type TDestination</returns>
        public static TDestination ParseEventDataToObject<TDestination>(EventData eventData, bool usePrivateCamelCaseSerializer = true)
            where TDestination : class
        {
            var settings = usePrivateCamelCaseSerializer ?
                new JsonSerializerSettings().GetJsonSerializerSettingsWithPrivateCamelCaseSerializer() :
                new JsonSerializerSettings();

            string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

            return JsonConvert.DeserializeObject<TDestination>(messageBody, settings);
        }
    }
}