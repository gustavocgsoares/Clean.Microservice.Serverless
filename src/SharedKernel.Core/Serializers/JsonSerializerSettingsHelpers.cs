using System.Collections.Generic;
using Clean.Microservice.Serverless.SharedKernel.Core.Serializers.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Serializers
{
    /// <summary>
    /// Helpers to helps get somes JsonSerializerSettings already defined.
    /// </summary>
    public static class JsonSerializerSettingsHelpers
    {
        /// <summary>
        /// Gets JsonSerializerSettings with following settings:
        /// <![CDATA[
        /// ContractResolver = new PrivateCamelCaseSerializer();
        /// Converters = new List<JsonConverter> { new StringEnumConverter() };
        /// NullValueHandling = NullValueHandling.Include;
        /// DefaultValueHandling = DefaultValueHandling.Populate
        /// ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        /// PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        /// ]]>
        /// </summary>
        /// <param name="settings">extension to current JsonSerializerSettings</param>
        /// <returns>JsonSerializerSettings with settings filled</returns>
        public static JsonSerializerSettings GetJsonSerializerSettingsWithPrivateCamelCaseSerializer(this JsonSerializerSettings settings)
        {
            var jsonSerializerSettings = settings ?? new JsonSerializerSettings();

            jsonSerializerSettings.ContractResolver = new PrivateCamelCaseSerializer();
            jsonSerializerSettings.Converters = new List<JsonConverter> { new StringEnumConverter() };
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            jsonSerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            return jsonSerializerSettings;
        }
    }
}
