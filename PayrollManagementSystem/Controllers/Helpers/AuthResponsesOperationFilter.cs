using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HisaabRakh.Helpers;

public class AuthResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true));

        var attributes = authAttributes?.ToList() ?? new List<object>();
        if (!attributes.Any()) return;
        if (attributes.FirstOrDefault(f => f.GetType() == typeof(AllowAnonymousAttribute)) != null) return;

        if (attributes.FirstOrDefault(f => f.GetType() == typeof(AuthorizeAttribute)) == null) return;
        var securityRequirement = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        };
        operation.Security = new List<OpenApiSecurityRequirement> {securityRequirement};
        operation.Responses.Add("401", new OpenApiResponse {Description = "Unauthorized"});
    }
}