using System;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectBool : UserControl
    {
        public bool Value = false;

        public InspectBool ( )
        {
            InitializeComponent ( );
        }

        private void checkBox1_CheckedChanged ( object sender, EventArgs e )
        {
            Value = checkBox1.Checked;
        }

        public void AlignToValue ( )
        {
            checkBox1.Checked = Value;
        }
    }
}