using System.Collections.Generic;
using System.Security.Claims;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Transaction
{
    /// <summary>
    /// Transaction flow interface.
    /// </summary>
    public interface ITransactionFlow
    {
        /// <summary>
        /// Gets user id.
        /// </summary>
        /// <value>
        /// User id.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets user name.
        /// </summary>
        /// <value>
        /// User name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets user nickname.
        /// </summary>
        /// <value>
        /// User nickname.
        /// </value>
        string Nickname { get; }

        /// <summary>
        /// Gets user email.
        /// </summary>
        /// <value>
        /// User email.
        /// </value>
        string Email { get; }

        /// <summary>
        /// Gets clientId.
        /// </summary>
        /// <value>
        /// ClientId.
        /// </value>
        string ClientId { get; }

        /// <summary>
        /// Gets scope.
        /// </summary>
        /// <value>
        /// Scope.
        /// </value>
        string Scope { get; }

        /// <summary>
        /// Gets role.
        /// </summary>
        /// <value>
        /// Role.
        /// </value>
        string Role { get; }

        /// <summary>
        /// Gets customer id.
        /// </summary>
        /// <value>
        /// Customer id.
        /// </value>
        string CustomerId { get; }

        /// <summary>
        /// Gets customer document.
        /// </summary>
        /// <value>
        /// Customer document.
        /// </value>
        string CustomerDocument { get; }

        /// <summary>
        /// Gets user application type.
        /// </summary>
        /// <example>Mobile, Postman and others.</example>
        /// <value>
        /// User application type.
        /// </value>
        string AppType { get; }

        /// <summary>
        /// Gets service machine name from running application.
        /// </summary>
        /// <value>
        /// Service machine name from running application.
        /// </value>
        string Machine { get; }

        /// <summary>
        /// Gets current culture.
        /// </summary>
        /// <value>
        /// Current culture.
        /// </value>
        string Culture { get; }

        /// <summary>
        /// Gets user host ip.
        /// </summary>
        /// <value>
        /// User host ip.
        /// </value>
        string HostIp { get; }

        /// <summary>
        /// Gets local ip where application is running.
        /// </summary>
        /// <value>
        /// Local ip where application is running.
        /// </value>
        string LocalIp { get; }

        /// <summary>
        /// Gets jwt token used from user.
        /// </summary>
        /// <value>
        /// Jwt token used from user.
        /// </value>
        string Token { get; }

        /// <summary>
        /// Gets transaction correlation id.
        /// </summary>
        /// <value>
        /// Transaction correlation id.
        /// </value>
        string CorrelationId { get; }

        /// <summary>
        /// Check if user is authenticated.
        /// </summary>
        /// <returns>True if user is authenticated. Otherwise false.</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Check if user is anonymous.
        /// </summary>
        /// <returns>True if user is anonymous. Otherwise false.</returns>
        bool IsAnonymous();

        /// <summary>
        /// Get list of claims from identity.
        /// </summary>
        /// <returns>List of claims from identity</returns>
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
