using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    public class UIScrollBarV : UIWidget
    {
        public UIButton Up, Down, Slide;
        public int Current
        {
            get
            {
                float ay = (Slide._WidY * (float)Vivid3D.App.AppInfo.H);
                float ey = WidH;
                float ar = (float)ay / (float)ey;
              
              float vv= (int)((float)Max * ar);
                if (vv < 5) vv = 5;
                return (int)vv;
            }
        }
        public int MaxValue = 0;
        public float Max = 0;
        public float ViewH = 0;
        public UIScrollBarV(int x, int y, int h, UIWidget root = null) : base(x, y, 15, h, "", root)
        {

            Slide = new UIButton(0, 0, 15, WidH - 30, "*", this);
           // Up = new UIButton(0, 0, 15, 15, "/\\",this);
            //Down = new UIButton(0, WidH - 15, 15, 15, "\\/", this);
            Slide.Dragged = (int mx, int my) =>
            {
            
                Slide._WidY = Slide._WidY + ((float)my / (float)App.AppInfo.H);
                
            };
        }
        public void Rebuild()
        {
            float yi = WidH / Max;
            Slide.WidH = WidH * yi;
            if (Slide.WidH > WidH) Slide.WidH = WidH;
            if (Slide.WidH < 5) Slide.WidH = 5;
            if (Slide.WidY < 0) Slide.WidY = 0;
            if (Slide._WidY < 0) Slide._WidY = 0;

            float asy = Slide._WidY * (float)App.AppInfo.H;
            if (asy > (WidH - Slide.WidH)) 
            {
               Slide.WidY = WidH - (Slide.WidH+4);
            }
            
        }
    }
}
