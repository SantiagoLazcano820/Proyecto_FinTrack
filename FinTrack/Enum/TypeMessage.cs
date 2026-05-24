namespace FinTrack.Core.Enum
{
    /// <summary>
    /// Representa un [Enum] para clasificar los tipos de mensajes del sistema.
    /// </summary>
    /// <remarks>
    /// Define la severidad o el propósito de las notificaciones enviadas en las respuestas de la API.
    /// </remarks>
    public enum TypeMessage
    {
        /// <summary>
        /// Indica que la operación se completó exitosamente.
        /// </summary>
        success,

        /// <summary>
        /// Indica una advertencia que no detiene el flujo pero requiere atención.
        /// </summary>
        warning,

        /// <summary>
        /// Proporciona información general del sistema al usuario.
        /// </summary>
        information,

        /// <summary>
        /// Indica que ocurrió un fallo crítico o un error de validación en el proceso.
        /// </summary>
        error
    }
}