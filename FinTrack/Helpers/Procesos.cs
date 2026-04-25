using System.Globalization;

namespace FinTrack.Core.Helpers
{
    public class Procesos
    {
        public static string ParseFechaFlexible(string? fechaTexto)
        {
            if (string.IsNullOrWhiteSpace(fechaTexto))
                return null;

            fechaTexto = fechaTexto.Replace(" ", " ").Trim();

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
