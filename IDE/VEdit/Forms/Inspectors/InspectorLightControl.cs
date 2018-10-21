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
    public partial class InspectorLightControl : InspectorBase
    {
        public Vivid3D.Lighting.GraphLight3D Light;
        public Inspectors.ValueTypes.InspectVec3 IPos;
        public Inspectors.ValueTypes.InspectVec3 IDiff;
        public Inspectors.ValueTypes.InspectVec3 ISpec;
        public Inspectors.ValueTypes.InspectFloat IRange;
        public Inspectors.ValueTypes.InspectBool IShadows;
        public InspectorLightControl()
        {
            InitializeComponent();
            IPos = new ValueTypes.InspectVec3();
            IPos.ValueName.Text = "Position";
            this.Controls.Add(IPos);
            IPos.Location = new Point(6, 6);
            IPos.Show();
            IDiff = new ValueTypes.InspectVec3();
            IDiff.ValueName.Text = "Diffuse";
            this.Controls.Add(IDiff);
            IDiff.Location = new Point(6, 52);
            IDiff.Show();
            ISpec = new ValueTypes.InspectVec3();
            ISpec.ValueName.Text = "Specular";
            this.Controls.Add(ISpec);
            ISpec.Location = new Point(6, 96);
            ISpec.Show();
            IRange = new ValueTypes.InspectFloat();
            IRange.ValueName.Text = "Range";
            this.Controls.Add(IRange);
            IRange.Location = new Point(6, 150);
            IRange.Show();
            IShadows = new ValueTypes.InspectBool();
            IShadows.ValueName.Text = "Shadows?";
            this.Controls.Add(IShadows);
            IShadows.Location = new Point(6, 180);
            IShadows.Show();





        }
        public override void AlignV()
        {
            Align();
        }
        public void Align()
        {
            IPos.Value = Light.LocalPos;
            IPos.AlignToValue();
            IDiff.Value = Light.Diff;
            IDiff.AlignToValue();
            ISpec.Value = Light.Spec;
            ISpec.AlignToValue();
            IRange.Value = Light.Range;
            IRange.AlignToValue();
            IShadows.Value = Light.CastShadows;
            IShadows.AlignToValue();
        }


        private void updateInspector_Tick(object sender, EventArgs e)
        {
            if (Inspecting == false) return;
            Console.WriteLine("Ipos:" + IDiff.Value);
          //  if (Light.Inspecting == false) return;
            Light.LocalPos = IPos.Value;
            Light.Diff = IDiff.Value;
            Light.Spec = ISpec.Value;
            Light.Range = IRange.Value;
            Light.CastShadows = IShadows.Value;
        }
    }
}
