using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors
{
    public class InspectorBase : UserControl
    {
        public bool Inspecting = true;

        public virtual void AlignV ( )
        {
        }

        public virtual void SetUI ( )
        {
        }
    }
}