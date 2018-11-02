using System;
using System.Drawing;
using System.Windows.Forms;
using Vivid3D.Scene;

namespace VividEdit.Forms.Inspectors
{
    public partial class InspectorEntity : InspectorBase
    {
        public GraphEntity3D Entity;
        public ValueTypes.InspectVec3 IPos;
        private Timer nt = new Timer();

        public InspectorEntity ( )
        {
            InitializeComponent ( );
            IPos = new ValueTypes.InspectVec3 ( );
            IPos.ValueName.Text = "Position";
            Controls.Add ( IPos );
            IPos.Location = new Point ( 6, 6 );
            IPos.Show ( );

            Size = new Size ( 512, 1000 );
        }

        public override void SetUI ( )
        {
            int mi = 0;
            int vy = 60;
            foreach ( Vivid3D.Data.VMesh m in Entity.Meshes )
            {
                Button b = new Button
                {
                    Text = "Mesh:" + mi + " Material Edit" + mi
                };
                mi++;
                b.Location = new Point ( 6, vy );
                b.Size = new Size ( 120, 25 );
                b.Click += ( sender, e ) =>
                {
                    VividEdit.VividED.Main.DockClassInspect.Inspect ( m.Mat );
                };
                Controls.Add ( b );
            }
        }

        private void B_Click ( object sender, EventArgs e )
        {
            //VividEdit.VividED.Main.DockClassInspect.Inspect(
            // )
            // throw new NotImplementedException();
        }

        private void Nt_Tick ( object sender, EventArgs e )
        {
            if ( Inspecting )
            {
                Entity.LocalPos = IPos.Value;
            }
            //throw new NotImplementedException();
        }

        public void StartTick ( )
        {
            nt.Interval = 40;
            nt.Tick += Nt_Tick1;
            nt.Enabled = true;
        }

        public override void AlignV ( )
        {
            Align ( );
        }

        private void Nt_Tick1 ( object sender, EventArgs e )
        {
            if ( Inspecting )
            {
                Entity.LocalPos = IPos.Value;
            }
        }

        public void Align ( )
        {
            IPos.Value = Entity.LocalPos;
            IPos.AlignToValue ( );
        }
    }
}