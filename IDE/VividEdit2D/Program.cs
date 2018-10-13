using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VividEdit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
         
            if (args.Length > 0)
            {
                Console.WriteLine("Running project:" + args[0]);
            }
            else
            {
                MessageBox.Show("The ide cannot be started directlly.\nUse the project manager to create/load projects.", "VividEdit");
                Environment.Exit(0);
            }
            VividEdit.BeginProject = args[0];
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VividEdit());
        }
    }
}
