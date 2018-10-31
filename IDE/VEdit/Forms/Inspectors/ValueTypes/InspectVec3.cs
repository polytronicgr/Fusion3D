using OpenTK;
using System;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectVec3 : UserControl
    {
        public Vector3 Value;

        public InspectVec3 ( )
        {
            InitializeComponent ( );
        }

        public void AlignToValue ( )
        {
            xBox.Text = Value.X.ToString ( );
            yBox.Text = Value.Y.ToString ( );
            zBox.Text = Value.Z.ToString ( );
        }

        private void label3_Click ( object sender , EventArgs e )
        {
        }

        private void InspectVec3_Load ( object sender , EventArgs e )
        {
        }

        private void xBox_TextChanged ( object sender , EventArgs e )
        {
            if ( xBox.Text.Length == 0 )
            {
                Value.X = 0;
                return;
            }
            if ( xBox.Text.Length == 1 )
            {
                if ( xBox.Text == "-" )
                {
                    Value.X = 0;
                    return;
                }
            }
            try
            {
                Value.X = float.Parse ( xBox.Text );
            }
            catch ( Exception )
            {
                Value.X = 0;
            }
        }

        private void yBox_TextChanged ( object sender , EventArgs e )
        {
            if ( yBox.Text.Length == 0 )
            {
                Value.Y = 0;
                return;
            }
            if ( yBox.Text.Length == 1 )
            {
                if ( yBox.Text == "-" )
                {
                    Value.Y = 0;
                    return;
                }
            }
            try
            {
                Value.Y = float.Parse ( yBox.Text );
            }
            catch ( Exception )
            {
                Value.Y = 0;
            }
        }

        private void zBox_TextChanged ( object sender , EventArgs e )
        {
            if ( zBox.Text.Length == 0 )
            {
                Value.Z = 0;
                return;
            }
            if ( zBox.Text.Length == 1 )
            {
                if ( zBox.Text == "-" )
                {
                    Value.Z = 0;
                    return;
                }
            }
            try
            {
                Value.Z = float.Parse ( zBox.Text );
            }
            catch ( Exception )
            {
                Value.Z = 0;
            }
        }
    }
}