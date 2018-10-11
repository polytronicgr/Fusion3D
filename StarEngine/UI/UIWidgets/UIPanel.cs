using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    public class UIPanel : UIWidget
        
    {
        public bool Flat = false;
        public UIPanel(int x,int y,int w,int h,string title,UIWidget top=null) : base(x,y,w,h,title,top)
        {
            
        }
        public override void Draw()
        {
            UISys.Skin().DrawPanel(this);
        }
    }
}
