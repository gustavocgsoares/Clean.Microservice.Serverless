using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Clean.Microservice.Serverless.Plugin.Logger;
using Newtonsoft.Json;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Function.Resources;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares
{
    /// <summary>
    /// Authentication middleware.
    /// </summary>
    public class AuthenticationMiddleware : HttpMiddleware
    {
        private readonly string issuer;
        private readonly string audience;
        private readonly string metadataAddress;
        private readonly IHttpContextAccessor acessor;
        private readonly bool isDebug;
        private readonly IServerlessLogger serverlessLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="serverlessLogger">Modal logger.</param>
        /// <param name="issuer">Authentication issuer.</param>
        /// <param name="audience">Authentication audience.</param>
        /// <param name="metadataAddress">Authentication metadata address.</param>
        /// <param name="acessor">See <see cref="IHttpContextAccessor"/>.</param>
        /// <param name="isDebug">True if environment is debug mode. Otherwise, false.</param>
        public AuthenticationMiddleware(
            IServerlessLogger serverlessLogger,
            string issuer,
            string audience,
            string metadataAddress,
            IHttpContextAccessor acessor,
            bool isDebug = false)
        {
            this.serverlessLogger = serverlessLogger;
            this.issuer = issuer;
            this.audience = audience;
            this.metadataAddress = metadataAddress;
            this.acessor = acessor;
            this.isDebug = isDebug;
        }

        /// <inheritdoc/>
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            CheckAuthHeader(context);

            int? statusCode = null;

            if (context.Response is ContentResult)
            {
                statusCode = ((ContentResult)context.Response).StatusCode;
            }

            if (!statusCode.HasValue && context.Response is ObjectResult)
            {
                statusCode = ((ObjectResult)context.Response).StatusCode;
            }

            if (statusCode.GetValueOrDefault() != 401 && statusCode.GetValueOrDefault() != 403)
            {
                await Next.InvokeAsync(context).ConfigureAwait(false);
            }
        }

        private static bool TokenValidateLifeTimeEnabled()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AUTH.TOKEN.VALIDATE.LIFETIME")))
            {
                return true;
            }

            if (Environment.GetEnvironmentVariable("AUTH.TOKEN.VALIDATE.LIFETIME").Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static bool HasInvalidAuthHeader(string authorization)
        {
            return authorization == null
                    || !authorization.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase);
        }

        private static void AddCustomClaims(string token, ClaimsPrincipal principal, HttpContext httpContext)
        {
            var headers = httpContext.Response.Headers;
            var httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>();
            var correlationId = new Microsoft.Extensions.Primitives.StringValues(Guid.Empty.ToString());
            var claimsAditionals = new ClaimsIdentity();

            headers.TryGetValue(Constants.HeaderNameCorrelationId, out correlationId);

            claimsAditionals.AddClaim(new Claim("token", token));
            claimsAditionals.AddClaim(new Claim("correlationId", correlationId.FirstOrDefault().ToString()));
            claimsAditionals.AddClaim(new Claim("host_ip", GetIpAddress(httpConnectionFeature)));
            claimsAditionals.AddClaim(new Claim("local_ip", GetLocalIpAddress(httpConnectionFeature)));

            principal.AddIdentity(claimsAditionals);
            httpContext.User = principal;
        }

        private static string GetLocalIpAddress(IHttpConnectionFeature httpConnectionFeature)
        {
            if (httpConnectionFeature == null)
            {
                return string.Empty;
            }

            return httpConnectionFeature?.LocalIpAddress?.ToString() ?? string.Empty;
        }

        private static string GetIpAddress(IHttpConnectionFeature httpConnectionFeature)
        {
            if (httpConnectionFeature == null)
            {
                return string.Empty;
            }

            return httpConnectionFeature?.RemoteIpAddress?.AddressFamily.ToString() ?? string.Empty;
        }

        private static ClaimsPrincipal ExtractClaimFromExternalJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));

            return claims;
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var validationParameter = GetValidationParameter();

            var handler = new JwtSecurityTokenHandler();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            handler.InboundClaimFilter.Clear();
            handler.InboundClaimTypeMap.Clear();
            handler.OutboundClaimTypeMap.Clear();

            return handler.ValidateToken(token, validationParameter, out var securityToken);
        }

        private TokenValidationParameters GetValidationParameter()
        {
            var config = GetConfigMananger()
                    .GetAwaiter()
                    .GetResult();

            return new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidAudience = audience,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = TokenValidateLifeTimeEnabled(),
                IssuerSigningKeys = config.SigningKeys
            };
        }

        private async Task<OpenIdConnectConfiguration> GetConfigMananger()
        {
            var documentRetriever = GetDocumentReceiver();

            var configurationManager = new
                            ConfigurationManager<OpenIdConnectConfiguration>(
                            metadataAddress,
                            new OpenIdConnectConfigurationRetriever(),
                            documentRetriever);

            var config = await configurationManager.GetConfigurationAsync(CancellationToken.None)
                                        .ConfigureAwait(false);

            return config;
        }

        private HttpDocumentRetriever GetDocumentReceiver()
        {
            return new HttpDocumentRetriever
            {
                RequireHttps = issuer.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
            };
        }

        private void CheckAuthHeader(IHttpFunctionContext context)
        {
            var request = context.Request;
            var authorization = (string)request.Headers["Authorization"];

            if (authorization == null || authorization?.Split(' ').Length != 2 || HasInvalidAuthHeader(authorization))
            {
                context.Response = new UnauthorizedObjectResult(string.Empty);
                return;
            }

            try
            {
                var token = authorization.Split(' ')[1];
                var principal = ExtractClaimFromExternalJwt(token);

                AddCustomClaims(authorization, principal, request.HttpContext);

                acessor.HttpContext = context.Request.HttpContext;
            }
            catch (Exception e)
            {
                serverlessLogger.LogError(e, $"[{nameof(AuthenticationMiddleware)}] {e.Message}");

                context.Response = GetForbiddenContent(e);
                return;
            }
        }

        private ContentResult GetForbiddenContent(Exception e)
        {
            var result = new
            {
                code = Messages.FORBIDDEN_ACCESS_CODE,
                message = string.Format(Messages.FORBIDDEN_ACCESS_MESSAGE, isDebug ? e.ToString() : string.Empty)
            };

            var responseContent = new ContentResult
            {
                StatusCode = 403,
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json"
            };

            return responseContent;
        }
    }
}
