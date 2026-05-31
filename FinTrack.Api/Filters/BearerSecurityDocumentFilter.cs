using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinTrack.Api.Filters
{
    /// <summary>
    /// Filtro de documento que registra la definición de seguridad Bearer en el documento de Swagger,
    /// habilitando el botón Authorize en la UI.
    /// </summary>
    public class BearerSecurityDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            if (openApiDoc.Components == null)
            {
                openApiDoc.Components = new OpenApiComponents();
            }

            if (openApiDoc.Components.SecuritySchemes == null)
            {
                openApiDoc.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
            }

            if (!openApiDoc.Components.SecuritySchemes.ContainsKey("Bearer"))
            {
                openApiDoc.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
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
