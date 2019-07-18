using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Transaction
{
    /// <summary>
    /// Transaction flow for azure function projects.
    /// </summary>
    public class FunctionTransactionFlow : TransactionFlow
    {
        private readonly IHttpContextAccessor accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionTransactionFlow"/> class.
        /// </summary>
        /// <param name="accessor">See <see cref="IHttpContextAccessor"/>.</param>
        public FunctionTransactionFlow(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        /// <inheritdoc/>
        public override bool IsAuthenticated()
        {
            if (IsIdentityExists())
            {
                return (accessor?.HttpContext?.User?.Identity?.IsAuthenticated).GetValueOrDefault();
            }

            return false;
        }

        /// <inheritdoc/>
        public override IEnumerable<Claim> GetClaimsIdentity()
        {
            return accessor?.HttpContext?.User?.Claims;
        }

        /// <inheritdoc/>
        protected override T GetClaimValueOrDefault<T>(string claim, T defaultValue = default(T))
        {
            if (IsClaimsExists())
            {
                T result = accessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == claim)?.Value as T;
                return result;
            }

            return defaultValue ?? default(T);
        }

        private bool IsClaimsExists()
        {
            return IsUserExists()
                && accessor?.HttpContext?.User?.Claims?.Count() > 0;
        }

        private bool IsIdentityExists()
        {
            return accessor?.HttpContext?.User?.Identity != null;
        }

        private bool IsUserExists()
        {
            return accessor?.HttpContext?.User != null;
        }
    }
}
