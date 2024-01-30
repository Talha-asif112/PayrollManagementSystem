using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayrollManagementSystem.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PayrollManagementSystem.Business;

public static class HelperExtensions
{
    public const int TokenLifetimeMinutes = 60;

    public static TokenValidationParameters GetValidationParameters(this IConfiguration configuration)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    }

    public static long GetUserId(this IHttpContextAccessor httpContext)
    {
        return long.Parse(httpContext.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? "0");
    }

    public static (string, long) GetRoleAndId(this HttpContext context)
    {
        var roleClaim = context.User.Claims.FirstOrDefault(f => f.Type == ClaimsIdentity.DefaultRoleClaimType);
        var userIdClaim = context.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
        if (roleClaim != null && userIdClaim != null)
        {
            return (roleClaim.Value, long.Parse(userIdClaim.Value));
        }

        return ("", 0);
    }

    public static long GetTenantId(this IHttpContextAccessor httpContext)
    {
        return long.Parse(httpContext.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value ??
                          "0");
    }

    public static (SymmetricSecurityKey securityKey, SigningCredentials credentials) JwtSecurityKeyAndCredentials(
        this IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        return (securityKey, credentials);
    }

    public static string GenerateJwt(this AppUser user, IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"];
        var (_, credentials) = configuration.JwtSecurityKeyAndCredentials();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.Hash, Guid.NewGuid().ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
        };

        var token = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddMinutes(TokenLifetimeMinutes),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string UpdateJwtToken(this HttpContext httpContext, IConfiguration configuration)
    {
        var authToken = httpContext.Request.Headers.Authorization.ToString();
        authToken = authToken.Replace("Bearer", "", true, CultureInfo.InvariantCulture).Trim();
        if (string.IsNullOrEmpty(authToken))
        {
            return "";
        }

        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
        if (jwtSecurityToken == null)
        {
            return "";
        }

        var (_, credentials) = configuration.JwtSecurityKeyAndCredentials();
        var issuer = configuration["Jwt:Issuer"];

        var token = new JwtSecurityToken(
            issuer,
            issuer,
            jwtSecurityToken.Claims,
            expires: DateTime.Now.AddMinutes(TokenLifetimeMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string ToJson(this Exception exception)
    {
        return JsonSerializer.Serialize(new
        {
            message = exception.Message,
            exceptionDetails = exception.StackTrace
        });
    }
}