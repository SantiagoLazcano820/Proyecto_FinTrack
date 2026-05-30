using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Exceptions;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;
using System.Net;

namespace FinTrack.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            if (userLogin.User.Trim().Equals(userLogin.Password.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var policyViolationMessage = "Inseguridad de credenciales: El nombre de usuario y la contraseña no pueden ser iguales.";
                throw new BusinessException(policyViolationMessage, HttpStatusCode.BadRequest);
            }
            var horaActual = DateTime.Now.Hour;
            var securityUser = await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
            if (securityUser != null && securityUser.Role == RoleType.StandardUser)
            {
                if (horaActual >= 4 && horaActual < 6)
                {
                    var maintenanceMessage = "Mantenimiento: Acceso disponible a partir de las 06:00 AM.";
                    throw new BusinessException(maintenanceMessage, HttpStatusCode.ServiceUnavailable);
                }
            }
            if (securityUser == null)
            {
                var authErrMessage = "El correo o la contraseña son incorrectos.";
                throw new BusinessException(authErrMessage, HttpStatusCode.Unauthorized);
            }
            return securityUser;
        }

        public async Task RegisterUser(Security security)
        {
            if (security.Login.Trim().Equals(security.Password.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException("El login y el password no pueden ser iguales.", HttpStatusCode.BadRequest);
            }
            await _unitOfWork.SecurityRepository.Insert(security);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}