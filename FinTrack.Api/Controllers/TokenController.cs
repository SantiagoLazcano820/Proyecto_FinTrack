using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Api.Controllers
{
    [Authorize(Roles = nameof(RoleType.Admin))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;

        public TokenController(IConfiguration configuration, ISecurityService securityService)
        {
            _configuration = configuration;
            _securityService = securityService;
        }

        /// <summary>
        /// Realiza la autenticación de un usuario y genera un token de acceso JWT válido si cumple las políticas del sistema.
        /// </summary>
        /// <remarks>
        /// Evalúa secuencialmente las reglas de negocio: inseguridad de duplicados, restricciones de horarios 
        /// de mantenimiento y coincidencia genérica de credenciales.
        /// </remarks>
        /// <param name="userLogin">Objeto que contiene las credenciales de inicio de sesión (Usuario y Contraseña).</param>
        /// <returns>Un token de seguridad firmado si la autenticación es exitosa.</returns>
        /// <response code="200">Autenticación exitosa y token JWT emitido de forma conforme</response>
        /// <response code="400">Petición errónea: El usuario y la contraseña enviados son idénticos</response>
        /// <response code="401">No autorizado: El correo o la contraseña son incorrectos</response>
        /// <response code="503">Servicio no disponible: Restricción horaria por mantenimiento del sistema</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(object))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            var securityUser = await _securityService.GetLoginByCredentials(userLogin);

            var token = GenerateToken(securityUser);
            return Ok(new { token });
        }

        /// <summary>
        /// Construye, configura y firma digitalmente el token de seguridad JWT inyectando los Claims de la entidad.
        /// </summary>
        private string GenerateToken(Security security)
        {
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim("Login", security.Login),
                new Claim("Name", security.Name),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(2) // Mantener los 2 minutos fijados por tu docente
            );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}