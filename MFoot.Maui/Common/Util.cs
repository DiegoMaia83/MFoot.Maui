using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Common
{
    public static class Util
    {
        public static string RetornarCorPorZona(string zona)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "G", "#008B8B" },
                { "D", "#48D1CC" },
                { "M", "#DAA520" },
                { "A", "#DC143C" }
            };

            if (dic.TryGetValue(zona, out string cor))
            {
                return cor;
            }
            else
            {
                return "#808080"; // Cor cinza padrão
            }
        }

        public static string RetornarCorResistencia(double resistencia)
        {
            string cor;

            if (resistencia > 70.00)
            {
                cor = "#00bf56";
            }
            else if (resistencia > 50.00)
            {
                cor = "#DAA520";
            }
            else if (resistencia > 20.00)
            {
                cor = "#f2a602";
            }
            else
            {
                cor = "#DC143C";
            }

            return cor;
        }

        public static string DateToSqLite(DateTime? date)
        {
            return (date ?? new DateTime(1994, 8, 1)).ToString("yyyy-MM-dd");
        }

        public static string DoubleToSqLite(double value)
        {
            return value.ToString("F2");
        }
    }
}
