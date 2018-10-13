using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;

using VividEdit.Forms;
namespace VividEdit
{
    public partial class VEdit 
    {

        public DockPanel MainDock;
        public Editor3D DockEdit3D;
        public AppGraph DockAppGraph;
        public ContentExplorer DockContentExplorer;

    }
}
