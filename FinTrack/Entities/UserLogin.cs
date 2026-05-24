namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa una [Entidad] de autenticación para el inicio de sesión de usuarios en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta entidad encapsula las credenciales primarias necesarias que el cliente debe enviar 
    /// para validar su identidad y generar un token de acceso seguro (JWT).
    /// </remarks>
    public class UserLogin
    {
        /// <summary>
        /// Nombre de usuario o dirección de correo electrónico del usuario registrado.
        /// </summary>
        /// <example>ejemplo@gmail.com</example>
        public string User { get; set; }

        /// <summary>
        /// Contraseña secreta correspondiente a la cuenta de usuario.
        /// </summary>
        /// <example>P@ssw0rd2026</example>
        public string Password { get; set; }
    }
}