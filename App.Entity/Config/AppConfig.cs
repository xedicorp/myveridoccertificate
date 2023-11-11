using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Config
{
    public class AppConfig
    {
        public static string Path { get { return "AppConfig"; } }

        public string AppName { get; set; } = string.Empty;
        public string AppUrl { get; set; } = string.Empty;
        public string AppLogoUrl { get; set; } = string.Empty;
    }
}
