using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Handlers;

namespace Warehouse.Server.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string BEARER_SECRET = "1f0971861e89567ba2182f11616e97fc5413982ac4074d44aec82d7a37b467f0";

        private IMediator _mediator;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMediator mediator)
            : base(options, logger, encoder, clock)
        {
            _mediator = mediator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }

            // HTTP header for HTTP credentials prompt
            Response.Headers["WWW-Authenticate"] = "Basic";

            // If no header, return failure
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            // Check for basic HTTP authentication
            ClaimsPrincipal principal = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                
                if (authHeader.Scheme.Equals("Bearer"))
                {
                    // Bearer <secret> scheme
                    AuthenticateBearer(authHeader.Parameter, out principal);
                } else if (authHeader.Scheme.Equals("Basic"))
                {
                    // Basic <Base64(username:password)> scheme
                    AuthenticateBasic(authHeader.Parameter, out principal);
                } else
                {
                    throw new Exception();
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private bool AuthenticateBearer(string parameter, out ClaimsPrincipal principal)
        {
            if (BEARER_SECRET.Equals(parameter))
            {
                var identity = new ClaimsIdentity(Scheme.Name);
                principal = new ClaimsPrincipal(identity);
                return true;
            } else
            {
                principal = null;
                return false;
            }
        }

        private bool AuthenticateBasic(string parameter, out ClaimsPrincipal principal)
        {
            var credentialBytes = Convert.FromBase64String(parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            var username = credentials[0];
            var password = credentials[1];

            var dataResponse = _mediator.Send(new AuthenticateCustomer.Request(username, password), CancellationToken.None);
            var result = dataResponse.Result;
            
            if (result.IsSuccess)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, result.Object.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Object.Name),
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                principal = new ClaimsPrincipal(identity);
                return true;
            } else
            {
                principal = null;
                return false;
            }
        }
    }
}
