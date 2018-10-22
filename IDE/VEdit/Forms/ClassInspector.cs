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
            if(cls is Vivid3D.Material.Material3D)
            {
                var cls3 = cls as Vivid3D.Material.Material3D;
                this.Text = "Inspecting Material";
                if (IC != null)
                {
                    this.Controls.Remove(IC);

                }
                var mi = new Inspectors.InspectorMaterial();
                Inspecting = mi;
                mi.Mat = cls3;
                mi.Align();
                mi.StartTick();
                IC = mi;
                mi.Location = new Point(0, 20);
                this.Controls.Add(mi);
                this.Show();
            }
            if(cls is Vivid3D.Scene.GraphEntity3D || cls is Vivid3D.Terrain.GraphTerrain)
            {
                var cls2 = cls as Vivid3D.Scene.GraphEntity3D;
                this.Text = "Inspecting:" + cls2.Name + "(3D Entity)";
                if (IC != null)
                {
                    this.Controls.Remove(IC);
                }
                var ei = new Inspectors.InspectorEntity();
                Inspecting = ei;
                ei.Entity = cls2;
                ei.Align();
                ei.StartTick();
                IC = ei;
                ei.Location = new Point(0, 20);
                this.Controls.Add(ei);
                this.Show();
            }
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
            IC.SetUI();
        }

        public void BeginInspect()
        {
            if (Inspecting != null)
            {
                Inspecting.Inspecting = true;
                Inspecting.AlignV();
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
                VividEdit.VividED.Main.BeginInspect();
            }
            else
            //    Console.WriteLine("Changed false");
            {
                VividEdit.VividED.Main.EndInspect();
            }
        }

        private void inspectBox_CheckStateChanged(object sender, EventArgs e)
        {

        }
    }
}
