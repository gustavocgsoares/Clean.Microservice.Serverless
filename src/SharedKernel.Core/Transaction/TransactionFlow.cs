using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using IdentityModel;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Transaction
{
    /// <summary>
    /// Base transaction flow.
    /// </summary>
    public abstract class TransactionFlow : ITransactionFlow
    {
        /// <summary>
        /// Gets user id using jwt subject field.
        /// </summary>
        /// <value>
        /// User id using jwt subject field.
        /// </value>
        public string Id => GetClaimValueOrDefault(JwtClaimTypes.Subject, string.Empty);

        /// <summary>
        /// Gets user name using jwt subject field.
        /// </summary>
        /// <value>
        /// User name using jwt subject field.
        /// </value>
        public string Name => GetClaimValueOrDefault(Constants.JwtCustomerName, "anonymous");

        /// <summary>
        /// Gets user nickname using jwt preferred user name field.
        /// </summary>
        /// <value>
        /// User nickname using jwt preferred user name field.
        /// </value>
        public string Nickname => GetClaimValueOrDefault(JwtClaimTypes.PreferredUserName, "anonymous");

        /// <summary>
        /// Gets user email using jwt email field.
        /// </summary>
        /// <value>
        /// User email using jwt email field.
        /// </value>
        public string Email => GetClaimValueOrDefault(JwtClaimTypes.Email, string.Empty);

        /// <summary>
        /// Gets clientId using jwt clientId field.
        /// </summary>
        /// <value>
        /// ClientId using jwt clientId field.
        /// </value>
        public string ClientId => GetClaimValueOrDefault(JwtClaimTypes.ClientId, string.Empty);

        /// <summary>
        /// Gets scope using jwt scope field.
        /// </summary>
        /// <value>
        /// Scope using jwt scope field.
        /// </value>
        public string Scope => GetClaimValueOrDefault(JwtClaimTypes.Scope, string.Empty);

        /// <summary>
        /// Gets role using jwt role field.
        /// </summary>
        /// <value>
        /// Role using jwt role field.
        /// </value>
        public string Role => GetClaimValueOrDefault(JwtClaimTypes.Role, string.Empty);

        /// <inheritdoc/>
        public string CustomerId => GetClaimValueOrDefault(Constants.JwtCustomerId, string.Empty);

        /// <inheritdoc/>
        public string CustomerDocument => GetClaimValueOrDefault(Constants.JwtCustomerDocument, string.Empty);

        /// <inheritdoc/>
        public string AppType => GetClaimValueOrDefault(Constants.JwtAppType, string.Empty);

        /// <inheritdoc/>
        public string Machine => GetClaimValueOrDefault(Constants.JwtMachine, string.Empty);

        /// <summary>
        /// Gets current culture from thread.
        /// </summary>
        /// <value>
        /// Current culture from thread.
        /// </value>
        public string Culture => Thread.CurrentThread?.CurrentCulture?.ToString();

        /// <inheritdoc/>
        public string HostIp => GetClaimValueOrDefault(Constants.JwtHostIp, string.Empty);

        /// <inheritdoc/>
        public string LocalIp => GetClaimValueOrDefault(Constants.JwtLocalIp, string.Empty);

        /// <inheritdoc/>
        public string Token => GetClaimValueOrDefault(Constants.JwtToken, string.Empty);

        /// <inheritdoc/>
        public string CorrelationId => GetClaimValueOrDefault(Constants.JwtCorrelationId, string.Empty);

        /// <inheritdoc/>
        public abstract bool IsAuthenticated();

        /// <inheritdoc/>
        public bool IsAnonymous()
        {
            return !IsAuthenticated();
        }

        /// <inheritdoc/>
        public abstract IEnumerable<Claim> GetClaimsIdentity();

        /// <summary>
        /// Get claim value or default.
        /// </summary>
        /// <typeparam name="TValue">Type of value data.</typeparam>
        /// <param name="claim">Name of claim.</param>
        /// <param name="defaultValue">Default value when claim not exists or value not found.</param>
        /// <returns>Claim value or default.</returns>
        protected abstract TValue GetClaimValueOrDefault<TValue>(string claim, TValue defaultValue = default(TValue))
            where TValue : class;
    }
}
