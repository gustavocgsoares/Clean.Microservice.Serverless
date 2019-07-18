using System.Text;
using System.Xml;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.Helpers
{
    /// <summary>
    /// Helpers to helps get somes XmlWriterSettings already defined.
    /// </summary>
    public static class XmlSerializerSettingsHelpers
    {
        /// <summary>
        /// Gets XmlSerializerSettings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="omitXmlDeclaration"></param>
        /// <param name="encoding"></param>
        /// <returns>XmlWriterSettings with settings filled</returns>
        public static XmlWriterSettings GetXmlSerializerSettings(
            this XmlWriterSettings settings,
            bool omitXmlDeclaration = true,
            Encoding encoding = null)
        {
            var xmlSerializerSettings = settings ?? new XmlWriterSettings();

            xmlSerializerSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            xmlSerializerSettings.OmitXmlDeclaration = omitXmlDeclaration;
            xmlSerializerSettings.Encoding = encoding ?? Encoding.UTF8;
            xmlSerializerSettings.DoNotEscapeUriAttributes = false;
            xmlSerializerSettings.NewLineChars = "\n";
            xmlSerializerSettings.IndentChars = "\t";
            xmlSerializerSettings.NewLineHandling = NewLineHandling.None;

            return xmlSerializerSettings;
        }
    }
}
