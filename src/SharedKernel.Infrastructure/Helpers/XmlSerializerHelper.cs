using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.Helpers
{
    /// <summary>
    /// Xml Helper to serializer
    /// </summary>
    public static class XmlSerializerHelper
    {
        /// <summary>
        /// Serialize objet to xml
        /// </summary>
        /// <typeparam name="TObject">Object type</typeparam>
        /// <param name="objectToSerializer">Object to serializer</param>
        /// <param name="xmlSettings">Define if must use custom xml settings with XmlWriterSettings().GetXmlSerializerSettings()</param>
        /// <returns>string with TObject serializated to XML</returns>
        public static string Serialize<TObject>(TObject objectToSerializer, XmlWriterSettings xmlSettings = null)
        {
            var serializer = new XmlSerializer(typeof(TObject));

            using (var ms = new MemoryStream())
            {
                var settings = xmlSettings == null ?
                    new XmlWriterSettings().GetXmlSerializerSettings() :
                    new XmlWriterSettings();

                using (var tw = XmlWriter.Create(ms, settings))
                {
                    serializer.Serialize(tw, objectToSerializer);

                    var reader = new StreamReader(ms);
                    ms.Position = 0;

                    return reader.ReadToEnd();
                }
            }
        }
    }
}
