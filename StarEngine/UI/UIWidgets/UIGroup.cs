using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.UI;
using Vivid3D.App;
namespace Vivid3D.UI.UIWidgets
{
    public class UIGroup : UIWidget
    {
        public UIGroup() : base(0, 0, AppInfo.W, AppInfo.H, "", null)
        {

        }
        public override bool InBounds()
        {
            return false;
        }
    }
}