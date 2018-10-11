using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    public delegate void Click();
    public class UIPatch
    {
        public int X, Y, W, H;
        public UIWidget Top = null;
        public Click Action;
        public int Life = 2;
    }
}
