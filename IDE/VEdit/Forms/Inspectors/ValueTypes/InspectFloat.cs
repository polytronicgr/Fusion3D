using System;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectFloat : UserControl
    {
        public float Value = 0;

        public InspectFloat ( )
        {
            InitializeComponent ( );
        }

        private void textBox1_TextChanged ( object sender, EventArgs e )
        {
            try
            {
                Value = float.Parse ( floatBox.Text );
            }
            catch ( Exception )
            {
                Value = 0;
            }
        }

        public void AlignToValue ( )
        {
            floatBox.Text = Value.ToString ( );
        }
    }
}