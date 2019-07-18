using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Serializers.Json
{
    /// <summary>
    /// Class to serializer, using JsonSerializerSettings, thats apply the following resolvers: Camel Case, Private Set
    /// </summary>
    public class PrivateCamelCaseSerializer : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Create properties serializer
        /// </summary>
        /// <param name="type">object type</param>
        /// <param name="memberSerialization">member serialization</param>
        /// <returns>List of JsonProperty</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .Select(x =>
                                {
                                    return new JsonProperty()
                                    {
                                        PropertyName = GetResolvedPropertyName(x.Name),
                                        PropertyType = x.PropertyType,
                                        Readable = true,
                                        ValueProvider = new AllPropertiesValueProvider(x),
                                        Writable = true
                                    };
                                })
                                .ToList();

            return props;
        }
    }
}
