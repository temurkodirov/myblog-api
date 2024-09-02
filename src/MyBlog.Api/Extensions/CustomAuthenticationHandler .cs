using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MyBlog.Api.Extensions;

public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                        ILoggerFactory logger,
                                        UrlEncoder encoder,
                                        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Token is missing");
        }

        try
        {
            // Validate token (use your own validation logic or service)
            var isValid = ValidateToken(token);

            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, "User") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
        }
    }

    private bool ValidateToken(string token)
    {
        // Implement your token validation logic here
        // Return true if the token is valid, otherwise false
        return true; // Placeholder
    }
}
