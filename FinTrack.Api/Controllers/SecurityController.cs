using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.Api.Controllers
{
    [Authorize(Roles = nameof(RoleType.Admin))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;

        public SecurityController(ISecurityService securityService, IMapper mapper)
        {
            _securityService = securityService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra y da de alta una nueva credencial de seguridad en la plataforma bajo estrictas normas de contraseñas.
        /// </summary>
        /// <remarks>
        /// Mapea el DTO de entrada a la entidad de negocio. Valida que las credenciales cumplan con la norma de 
        /// no duplicidad (RN-10) antes de guardarlo.
        /// </remarks>
        /// <param name="securityDto">Objeto de transferencia de datos con el esquema de registro.</param>
        /// <returns>Una estructura de tipo <see cref="ApiResponse{SecurityDto}"/> con el registro completado.</returns>
        /// <response code="200">La credencial de acceso fue registrada exitosamente</response>
        /// <response code="400">Petición errónea: El login y la contraseña ingresados son idénticos infringiendo la RN-10</response>
        /// <response code="401">No autorizado: Se requiere un token de autenticación JWT válido en la cabecera</response>
        /// <response code="403">Acceso denegado: El rol del usuario actual no posee privilegios de Administrador</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<SecurityDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint de prueba técnica para validar la vigencia de la conexión y las políticas de autorización.
        /// </summary>
        /// <remarks>
        /// Método de diagnóstico de seguridad. Solo responde con éxito si el token JWT enviado pertenece a un Administrador activo.
        /// </remarks>
        /// <returns>Un diccionario con un mensaje de éxito transaccional.</returns>
        /// <response code="200">Conexión exitosa, token y rol verificados correctamente</response>
        /// <response code="401">No autorizado, token ausente o expirado</response>
        /// <response code="403">Acceso denegado para el rol suministrado</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Dictionary<string, string>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Get()
        {
            Dictionary<string, string> dic = new();
            dic.Add("Message", "Coneccion exitosa");
            return Ok(dic);
        }
    }
}