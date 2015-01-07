using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PWMeditation
{
    static class Program
    {
        public static List<Settings> SettingsList = new List<Settings>();
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args));
            Locale.Localization.Initialize();
            MeditationLogic.Enabled = false;
        }
    }
}
