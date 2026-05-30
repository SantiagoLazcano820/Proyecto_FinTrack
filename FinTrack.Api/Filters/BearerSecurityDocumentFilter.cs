using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinTrack.Api.Filters
{
    /// <summary>
    /// Filtro de documento que registra la definición de seguridad Bearer en el documento de Swagger,
    /// habilitando el botón Authorize en la UI.
    /// </summary>
    public class BearerSecurityDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Components ??= new OpenApiComponents();
            swaggerDoc.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

            if (!swaggerDoc.Components.SecuritySchemes.ContainsKey("Bearer"))
            {
                swaggerDoc.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticación JWT usando el esquema Bearer. Escribe: 'Bearer {tu_token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            }
        }
    }
}
