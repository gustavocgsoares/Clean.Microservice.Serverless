namespace Clean.Microservice.Serverless.SharedKernel.Core.Transaction
{
    /// <summary>
    /// Helper constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Digital Bank SmartId claim
        /// </summary>
        public const string JwtCustomerName = "Nam";

        /// <summary>
        /// Customer id claim
        /// </summary>
        public const string JwtCustomerId = "Sid";

        /// <summary>
        /// Customer account claim
        /// </summary>
        public const string JwtCustomerAccountId = "Acc";

        /// <summary>
        /// Customer Document claim
        /// </summary>
        public const string JwtCustomerDocument = "Iss";

        /// <summary>
        /// Application Type claim
        /// </summary>
        public const string JwtAppType = "app_type";

        /// <summary>
        /// Machine name claim
        /// </summary>
        public const string JwtMachine = "machine";

        /// <summary>
        /// Host ip claim
        /// </summary>
        public const string JwtHostIp = "host_ip";

        /// <summary>
        /// Local ip claim
        /// </summary>
        public const string JwtLocalIp = "local_ip";

        /// <summary>
        /// Auth Token
        /// </summary>
        public const string JwtToken = "token";

        /// <summary>
        /// Auth Token
        /// </summary>
        public const string JwtCorrelationId = "correlationId";

        /// <summary>
        /// Customer Document Length
        /// </summary>
        public const int JwtCustomerDocumentLength = 11;
    }
}
