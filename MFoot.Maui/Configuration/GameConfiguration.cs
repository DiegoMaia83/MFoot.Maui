using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Configuration
{
    public static class GameConfiguration
    {
        public static string TemporadaAtual
        {
            get => Preferences.Get("temporadaAtual", "0");
            set => Preferences.Set("temporadaAtual", value);
        }

        public static string DataAtual
        {
            get => Preferences.Get("dataAtual", "2024-01-31");
            set => Preferences.Set("dataAtual", value);
        }

        public static string AnoAtual
        {
            get => Preferences.Get("dataAtual", "2024");
            set => Preferences.Set("dataAtual", value);
        }

        public static string RodadaAtual
        {
            get => Preferences.Get("dataAtual", "2024");
            set => Preferences.Set("dataAtual", value);
        }
    }
}