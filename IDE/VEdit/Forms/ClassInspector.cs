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
namespace VividEdit.Forms
{
    public partial class ClassInspector : DockContent
    {
        public Inspectors.InspectorBase Inspecting;
        public ClassInspector()
        {
            InitializeComponent();
        }
        public Inspectors.InspectorBase IC = null;
        public void Inspect(object cls)
        {
            if(cls is Vivid3D.Lighting.GraphLight3D)
            {
                var cl = cls as Vivid3D.Lighting.GraphLight3D;
                this.Text = "Inspecting:" + cl.Name + "(3D Light)";


                if (IC != null)
                {
                    this.Controls.Remove(IC);
                }
                var li = new Inspectors.InspectorLightControl();
                Inspecting = li;
                li.Light = cls as Vivid3D.Lighting.GraphLight3D;
                li.Align();
                IC = li;
                li.Location = new Point(0, 20);
                this.Controls.Add(li);
                this.Show();

            }
        }

        public void BeginInspect()
        {
            if (Inspecting != null)
            {
                Inspecting.Inspecting = true;
            }
        }
        public void EndInspect()
        {
            if (Inspecting != null)
            {
                Inspecting.Inspecting = false;
            }
        }
        private void inspectBox_CheckedChanged(object sender, EventArgs e)
        {
            
            //
            if (inspectBox.Checked)
            {
                Console.WriteLine("Changed True");
                VEdit.VEdit.Main.BeginInspect();
            }
            else
            //    Console.WriteLine("Changed false");
            {
                VEdit.VEdit.Main.EndInspect();
            }
        }

        private void inspectBox_CheckStateChanged(object sender, EventArgs e)
        {

        }
    }
}
