using System.Globalization;

namespace FinTrack.Core.Helpers
{
    /// <summary>
    /// Representa una [Entidad] helper de procesos y utilidades lógicas globales en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase estática provee herramientas y algoritmos de soporte técnico recurrentes, tal como la normalización, 
    /// validación y conversión de formatos de tipos complejos dentro de la aplicación.
    /// </remarks>
    public class Procesos
    {
        /// <summary>
        /// Evalúa y analiza un texto de entrada para transformarlo de manera flexible en una fecha estructurada válida.
        /// </summary>
        /// <remarks>
        /// Intenta emparejar la cadena recibida contra una colección estricta de formatos comunes de fecha (tanto de 12 horas, 24 horas, esquemas regionales latinos y anglosajones) 
        /// utilizando la configuración cultural local de Bolivia (es-BO). Si tiene éxito, devuelve la fecha normalizada en formato dd/MM/yyyy.
        /// </remarks>
        /// <param name="fechaTexto">La cadena de texto original que representa la fecha ingresada por el usuario o cliente.</param>
        /// <returns>Una cadena de texto con la fecha en formato estandarizado "dd/MM/yyyy", o nulo si no se pudo parsear contra ningún formato válido.</returns>
        /// <example>23/05/2026</example>
        public static string ParseFechaFlexible(string? fechaTexto)
        {
            if (string.IsNullOrWhiteSpace(fechaTexto))
                return null;

            fechaTexto = fechaTexto.Replace(" ", " ").Trim();

            string[] formatos = new[]
            {
                //Solo Fecha dd/mm/yyyy
                "d-M-yyyy",                 // 12 horas con a. m. / p. m.
                "dd-MM-yyyy",               // otra versión con 12h
                "dd/MM/yyyy",

                //Fecha y hora dd/mm/yyyy
                "d-M-yyyy h:mm:ss tt",      // 12 horas con a. m. / p. m.
                "d-M-yyyy H:mm:ss",         // 24 horas
                "d-M-yyyy HH:mm:ss",        // 24 horas (doble dígito en hora)
                "dd-MM-yyyy HH:mm:ss",      // alternativa más común en servidores
                "dd-MM-yyyy h:mm:ss tt",    // otra versión con 12h

                //Solo Fecha mm/dd/yyyy
                "M-d-yyyy",                 // 12 horas con a. m. / p. m.
                "MM-dd-yyyy",               // otra versión con 12h

                "M-d-yyyy h:mm:ss tt",      // 12 horas con a. m. / p. m.
                "M-d-yyyy H:mm:ss",         // 24 horas
                "M-d-yyyy HH:mm:ss",        // 24 horas (doble dígito en hora)
                "MM-dd-yyyy HH:mm:ss",      // alternativa más común en servidores
                "MM-dd-yyyy h:mm:ss tt"     // otra versión con 12h
            };


            var cultura = new CultureInfo("es-BO");

            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(fechaTexto, formato, cultura, DateTimeStyles.None, out DateTime resultado))
                {
                    return resultado.ToString("dd/MM/yyyy");
                }
            }

            Console.WriteLine("Fecha no válida: " + fechaTexto);
            return null;
        }
    }
}