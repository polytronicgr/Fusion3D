using System;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit.Forms
{
    public partial class ConsoleView : DockContent
    {
        public static void Log(string info, string type)
        {
            Main.LogMsg(info, type);
        }

        public static ConsoleView Main;

        public ConsoleView()
        {
            InitializeComponent();
            Main = this;
        }

        public void LogMsg(string info, string type)
        {
            logBox.Text = logBox.Text + DateTime.Now.ToShortTimeString() + ": " + type + ":" + info + "\n";
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
        }
    }
}