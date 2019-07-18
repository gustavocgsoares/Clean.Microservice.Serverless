using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.Plugin.Logger.Helpers
{
    [SuppressMessage("Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Not applicable")]
    internal class LoggerHelper
    {
        public static object ConvertToJson(string data, IServerlessLogger serverlessLogger)
        {
            try
            {
                return string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject(data);
            }
            catch (Exception ex)
            {
                serverlessLogger.LogError(ex, $"{nameof(LoggerHelper)} -> Fail using {nameof(ConvertToJson)}: data -> {data}. message: {ex.Message}");
                return data;
            }
        }
    }
}
