using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors
{
    public partial class InspectorMaterial : InspectorBase
    {
        public Vivid3D.Material.Material3D Mat;
        public ValueTypes.InspectVec3 Diff;
        public ValueTypes.InspectVec3 Spec;
        public ValueTypes.InspectFloat Shine;
        public ValueTypes.InspectTexture ColTex;
        public InspectorMaterial()
        {
            InitializeComponent();
            Diff = new ValueTypes.InspectVec3();
            Diff.ValueName.Text = "Diffuse";
            this.Controls.Add(Diff);
            Diff.Location = new Point(6, 6);
            Diff.Show();

            Spec = new ValueTypes.InspectVec3();
            Spec.ValueName.Text = "Spec";
            this.Controls.Add(Spec);
            Spec.Location = new Point(6, 45);
            Spec.Show();

            Shine = new ValueTypes.InspectFloat();
            Shine.ValueName.Text = "Shine";
            this.Controls.Add(Shine);
            Shine.Location = new Point(6, 106);
            Shine.Show();

            ColTex = new ValueTypes.InspectTexture();
            ColTex.ValueName.Text = "Diffuse Texture";
            this.Controls.Add(ColTex);
            ColTex.Location = new Point(6, 170);
            ColTex.Show();
            Size = new Size(1000, 1000);


        }
        Timer nt = new Timer();
        public void StartTick()
        {
            nt.Interval = 40;
            nt.Tick += Nt_Tick;
            nt.Enabled = true;
        }
        public override void AlignV()
        {
            Align();
        }
        public void Align()
        {
            Diff.Value = Mat.Diff;
            Spec.Value = Mat.Spec;
            Shine.Value = Mat.Shine;
            ColTex.Value = Mat.TCol;
            Diff.AlignToValue();
            Spec.AlignToValue();
            Shine.AlignToValue();
            ColTex.AlignToValue();
        }
        private void Nt_Tick(object sender, EventArgs e)
        {
            if (Inspecting)
            {

                Mat.Diff = Diff.Value;
                Mat.Spec = Spec.Value;
                Mat.Shine = Shine.Value;
                Mat.TCol = ColTex.Value;

            }
        }
    }
}
