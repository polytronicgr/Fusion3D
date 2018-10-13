using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
namespace VividEdit
{
    public partial class VividEdit : Form
    {

        public static string BeginProject = "";

        public VividEdit()
        {
            
            InitializeComponent();
            
            MainDock = new DockPanel();
            MainDock.Dock = DockStyle.Fill;
            this.Controls.Add(MainDock);
           // DockEdit3D = new Forms.Editor3D();
            //DockEdit3D.Show();
           // DockEdit3D.Show(MainDock, DockState.Document);


        }

        private void VividEdit_Load(object sender, EventArgs e)
        {
            DockEdit3D = new Forms.Editor3D();
            DockEdit3D.Show(MainDock, DockState.Document);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

           
        }
    }
}
