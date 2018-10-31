using System;
using System.Windows.Forms;

namespace VividEdit
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            VividED.BeginProject = "ScopeNine - Tech Demo One";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VividED());
        }
    }
}