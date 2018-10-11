using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    public delegate void MenuClick();
    public class UIMenuItem : UIWidget
    {
        public UIPanel subp = null;
        public MenuClick Action = null;
        public UIMenuItem(string text, UIWidget menu, bool panel) : base(0, 0, 0, 0, text, menu)
        {
            if (panel)
            {
                subp = new UIPanel(5, 25, 0, 0, "", this);
                subp.Flat = true;
                subp.WidOpen = false;
            }
        }
        bool over = false;
        public UIMenuItem AddItem(string text)
        {
            int ys = 15;
            if (subp != null)
            {
                foreach (var i in Sub[0].Sub)
                {
                    ys += 25;


                }
            }

            UIMenuItem ni = new UIMenuItem(text, subp,false)
            {
                WidY = ys,
                WidW = UISys.Skin().SmallFont.Width(text) + 5,
                WidH = 25
            };
            if (ni.WidY < 25) ni.WidY = 25;

            AppResize(App.AppInfo.W, App.AppInfo.H);
            return ni;

        }
        public override void AppResize(int w, int h)
        {
            
            int ys = 10;
            int mw = -5;
            if (Sub.Count > 0)
            {
                if (Sub[0].Sub.Count > 0)
                {
                    foreach (var i in Sub[0].Sub)
                    {
                        var sw = UISys.Skin().SmallFont.Width(i.Name) + 5;
                        if (sw > mw) mw = sw;
                        i.WidY = ys;
                        ys += 25;
                    }
                }
            }
            if (subp != null)
            {
                subp.WidH = ys;
                subp.WidW = mw;
            }
                base.Resized();
        }
        public override void Draw()
        {
            if (over ==false)
            {
                UISys.Skin().DrawText((int)WidX, (int)WidY, Name, new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 1.0f));
            }else
            {
                UISys.Skin().DrawText((int)WidX, (int)WidY, Name, new OpenTK.Vector4(0.95f, 0.95f, 0.95f, 1.0f));
            }
        }
        public override void OnMouseDown(UIMouseButton b)
        {
            if (subp != null)
            {
                if (subp.WidOpen == true)
                {
                    subp.WidOpen = false;
                }
                else
                {
                    subp.WidOpen = true;
                }
            }
            else
            {
                if (Action != null)
                {
                    Action();
                }
            }
        }
        public override void OnEnter()
        {
            over = true;
        }
        public override void OnLeave()
        {
            over = false;
        }
    }
}
