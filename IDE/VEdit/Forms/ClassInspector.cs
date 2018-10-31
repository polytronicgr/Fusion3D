using System;
using System.Drawing;
using System.Windows.Forms;
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
            Controls.Clear();
            Label header = new Label();
            header.Text = "Inspecting:" + cls.GetType().Name;
            header.Location = new Point(5, 5);
            header.Size = new Size(120, 25);
            Controls.Add(header);

            var type = cls.GetType();

            int propy = 30;

            foreach (var prop in type.GetProperties())
            {
                Label prop_header = new Label();
                prop_header.Text = prop.Name;
                prop_header.Location = new Point(5, propy);
                prop_header.Size = new Size(250, 25);

                if (prop.PropertyType == typeof(float))
                {
                    Label val = new Label();
                    val.Text = prop.Name;
                    val.Location = new Point(5, propy + 25);
                    val.Size = new Size(40, 25);

                    var boxv = new TextBox();

                    boxv.Location = new Point(60, propy + 22);
                    boxv.Size = new Size(120, 25);

                    var v3 = (float)prop.GetAccessors()[0].Invoke(cls, null);
                    var sm = prop.GetSetMethod();

                    object[] pp = new object[1];

                    boxv.Text = v3.ToString();

                    void c_val(object o, EventArgs e)
                    {
                        try
                        {
                            v3 = float.Parse(boxv.Text);
                        }
                        catch
                        {
                            v3 = 0;
                        }
                        if (sm != null)
                        {
                            pp[0] = v3;
                            sm.Invoke(cls, pp);
                        }
                    }

                    boxv.TextChanged += c_val;

                    Controls.Add(val);
                    Controls.Add(boxv);
                }
                else if (prop.PropertyType == typeof(OpenTK.Vector3))
                {
                    Label labx, laby, labz;
                    labx = new Label();
                    laby = new Label();
                    labz = new Label();

                    TextBox boxx, boxy, boxz;

                    boxx = new TextBox();
                    boxy = new TextBox();
                    boxz = new TextBox();

                    labx.Location = new Point(5, propy + 25);
                    laby.Location = new Point(120, propy + 25);
                    labz.Location = new Point(235, propy + 25);

                    labx.Size = new Size(15, 25);
                    laby.Size = new Size(15, 25);
                    labz.Size = new Size(15, 25);
                    labx.Text = "X";
                    laby.Text = "Y";
                    labz.Text = "Z";

                    var v3 = (OpenTK.Vector3)prop.GetAccessors()[0].Invoke(cls, null);
                    var sm = prop.GetSetMethod();

                    object[] pp = new object[1];

                    Controls.Add(labx); Controls.Add(laby); Controls.Add(labz);

                    void v3_cx(object s, EventArgs a)
                    {
                        try
                        {
                            v3.X = float.Parse(boxx.Text);
                        }
                        catch
                        {
                            v3.X = 0;
                        }
                        if (sm != null)
                        {
                            pp[0] = v3;
                            sm.Invoke(cls, pp);
                        }
                        //prop.GetAccessors()[1].Invoke(cls, pp);
                    }

                    void v3_cy(object s, EventArgs e)
                    {
                        try
                        {
                            v3.Y = float.Parse(boxy.Text);
                        }
                        catch
                        {
                            v3.Y = 0;
                        }
                        if (sm != null)
                        {
                            pp[0] = v3;
                            sm.Invoke(cls, pp);
                        }
                    }

                    void v3_cz(object s, EventArgs e)
                    {
                        try
                        {
                            v3.Z = float.Parse(boxz.Text);
                        }
                        catch
                        {
                            v3.Z = 0;
                        }
                        if (sm != null)
                        {
                            pp[0] = v3;
                            sm.Invoke(cls, pp);
                        }
                    }

                    boxx.TextChanged += v3_cx;
                    boxy.TextChanged += v3_cy;
                    boxz.TextChanged += v3_cz;

                    boxx.Location = new Point(20, propy + 22);
                    boxy.Location = new Point(135, propy + 22);
                    boxz.Location = new Point(250, propy + 22);

                    boxx.Size = new Size(80, 25);
                    boxy.Size = new Size(80, 25);
                    boxz.Size = new Size(80, 25);

                    Controls.Add(boxx); Controls.Add(boxy); Controls.Add(boxz);

                    boxx.Text = v3.X.ToString(); boxy.Text = v3.Y.ToString(); boxz.Text = v3.Z.ToString();

                    this.Controls.Add(prop_header);
                }
                else
                {
                    continue;
                }

                propy += 50;
            }

            return;
            if (cls is Vivid3D.Material.Material3D)
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
            if (cls is Vivid3D.Scene.GraphEntity3D || cls is Vivid3D.Terrain.GraphTerrain)
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
            if (cls is Vivid3D.Lighting.GraphLight3D)
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

        private void Boxx_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            /*
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
            */
        }

        private void inspectBox_CheckStateChanged(object sender, EventArgs e)
        {
        }
    }
}