using Microsoft.IdentityModel.Tokens;

namespace MyBlog.Api.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token is missing");
            return;
        }

        try
        {
            // Validate token (use your own validation logic or service)
            var isValid = ValidateToken(token);

            if (!isValid)
            {
                throw new SecurityTokenInvalidSignatureException("Invalid token signature");
            }

            // Continue processing
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Token validation failed: {ex.Message}");
        }
    }

    private bool ValidateToken(string token)
    {
        // Implement your token validation logic here
        // Return true if the token is valid, otherwise false
        return true; // Placeholder
    }
}
