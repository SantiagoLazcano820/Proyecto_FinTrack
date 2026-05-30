using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinTrack.Api.Filters
{
    /// <summary>
    /// Filtro de operación para Swagger que agrega automáticamente el requisito de autorización Bearer JWT
    /// a todos los endpoints protegidos con [Authorize].
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Verificar si el endpoint tiene [Authorize]
            var hasAuthorize = context.MethodInfo.DeclaringType != null &&
                (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                 context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            // Verificar si tiene [AllowAnonymous]
            var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            if (!hasAuthorize || hasAllowAnonymous)
                return;

            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [bearerScheme] = Array.Empty<string>()
                }
            };
        }
    }
}
