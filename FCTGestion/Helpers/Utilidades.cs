using System.Globalization;
using System.Text;
using System.Linq;

namespace FCTGestion.Helpers
{
    public static class Utilidades
    {
        public static string NormalizarNombreUsuario(string nombre)
        {
            var normalizado = nombre.Normalize(NormalizationForm.FormD);
            var caracteresValidos = normalizado.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            var limpio = new string(caracteresValidos.ToArray());

            return limpio.Replace(" ", "_").Replace("-", "_");
        }
    }
}
