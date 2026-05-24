using System.Threading.Tasks;
using FinTrack.Core.Entities;
using FinTrack.Core.CustomEntities;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato especializado de repositorio para la [Entidad] de credenciales Security.
    /// </summary>
    /// <remarks>
    /// Provee firmas lógicas orientadas a la validación de accesos seguros y recuperación de perfiles de autenticación.
    /// </remarks>
    public interface ISecurityRepository : IBaseRepository<Security>
    {
        /// <summary>
        /// Obtiene de forma asíncrona una entidad Security que coincida exactamente con las credenciales de inicio de sesión provistas.
        /// </summary>
        /// <param name="login">Objeto con las credenciales (User y Password) enviadas por el cliente.</param>
        /// <returns>La [Entidad] <see cref="Security"/> si coincide la autenticación, de lo contrario null.</returns>
        Task<Security> GetLoginByCredentials(UserLogin login);
    }
}