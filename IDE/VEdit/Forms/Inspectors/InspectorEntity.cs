using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vivid3D.Scene;
namespace VividEdit.Forms.Inspectors
{
    public partial class InspectorEntity : InspectorBase
    {
        public GraphEntity3D Entity;
        public ValueTypes.InspectVec3 IPos;
        Timer nt = new Timer();

        public InspectorEntity()
        {
            InitializeComponent();
            IPos = new ValueTypes.InspectVec3();
            IPos.ValueName.Text = "Position";
            this.Controls.Add(IPos);
            IPos.Location = new Point(6, 6);
            IPos.Show();
       
            this.Size = new Size(512, 1000);
        }

        private void Nt_Tick(object sender, EventArgs e)
        {
            if (Inspecting)
            {

                Entity.LocalPos = IPos.Value;

            }
            //throw new NotImplementedException();
        }
        public void StartTick()
        {
            nt.Interval = 40;
            nt.Tick += Nt_Tick1;
            nt.Enabled = true;
        }
        public override void AlignV()
        {
            Align();
        }
        private void Nt_Tick1(object sender, EventArgs e)
        {
            if (Inspecting)
            {

                Entity.LocalPos = IPos.Value;

            }
        }

        public void Align()
        {
            IPos.Value = Entity.LocalPos;
            IPos.AlignToValue();
        }
    }
}
