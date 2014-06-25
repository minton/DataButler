using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DataButler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupRegistry();
            if (DontRun()) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Restore(Environment.GetCommandLineArgs()[1], Environment.GetCommandLineArgs()[2]));
        }

        static bool DontRun()
        {
            if (Environment.GetCommandLineArgs().Count() < 2) return true;
            if (File.Exists(Environment.GetCommandLineArgs()[1])) return false;
            MessageBox.Show("Invalid database backup.", "DataButler");
            return true;
        }

        static void SetupRegistry()
        {
            var dataButlerPath = string.Format(@"{0} ""%1""", Process.GetCurrentProcess().MainModule.FileName);
            SetupContextMenuAssociations(dataButlerPath);
        }

        static void SetupContextMenuAssociations(string dataButlerPath)
        {
            var bakHive = @"Software\Classes\.bak";
            if (!HiveExists(bakHive)) CreateHive(bakHive);
            SetDefaultHiveValue(bakHive, "DataButler");

            var commandHive = @"Software\Classes\DataButler\shell\Restore via DataButler\command";
            if (!HiveExists(commandHive)) CreateHive(commandHive);
            SetDefaultHiveValue(commandHive, dataButlerPath);

            var openCommandHive = @"Software\Classes\DataButler\shell\open\command";
            if (!HiveExists(openCommandHive)) CreateHive(openCommandHive);
            SetDefaultHiveValue(openCommandHive, dataButlerPath);
        }

        static bool HiveExists(string hive)
        {
            var softwareKey = Registry.CurrentUser.OpenSubKey(hive);
            return softwareKey != null;
        }

        static void CreateHive(string bakHive)
        {
            Registry.CurrentUser.CreateSubKey(bakHive);
        }

        static void SetDefaultHiveValue(string hive, string defaultValue)
        {
            var key = Registry.CurrentUser.OpenSubKey(hive, RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue(null, defaultValue);
        }
    }
}
