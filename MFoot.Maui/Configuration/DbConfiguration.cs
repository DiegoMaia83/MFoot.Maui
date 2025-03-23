using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Configuration
{
    public static class DbConfiguration
    {
        public static string CurrentDb
        {
            get => Preferences.Get("currentGameDatabase", "default.db");
            set => Preferences.Set("currentGameDatabase", value);
        }

        public static string PathtDb
        {
            get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MFoot");
        }
    }
}
