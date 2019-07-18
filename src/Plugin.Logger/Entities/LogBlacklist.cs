using System.Collections.Generic;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    /// <summary>
    /// Logger blacklist with sensitive fields to be masked.
    /// </summary>
    public class LogBlacklist : List<string>
    {
    }
}
