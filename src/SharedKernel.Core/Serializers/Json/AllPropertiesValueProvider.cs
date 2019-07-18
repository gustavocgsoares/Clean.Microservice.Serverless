using System.Reflection;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Serializers.Json
{
    /// <summary>
    /// Get properties info to serializer
    /// </summary>
    public class AllPropertiesValueProvider : Newtonsoft.Json.Serialization.IValueProvider
    {
        private PropertyInfo propertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllPropertiesValueProvider"/> class.
        /// </summary>
        /// <param name="p"></param>
        public AllPropertiesValueProvider(PropertyInfo p)
        {
            propertyInfo = p;
        }

        /// <summary>
        /// Gets value
        /// </summary>
        /// <param name="target"></param>
        /// <returns>object</returns>
        public object GetValue(object target)
        {
            return propertyInfo.GetValue(target);
        }

        /// <summary>
        /// Sets value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue(object target, object value)
        {
            propertyInfo.SetValue(target, value, null);
        }
    }
}
