using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    public delegate void OpenNode(UIItem i);
    public class UITreeView : UIWidget
    {
        public UIScrollBarV Scroll;
        public OpenNode Open = null;
        public UITreeView(int x, int y, int w, int h, string title, UIWidget root = null) : base(x, y, w, h, title, root)
        {
            Scroll = new UIScrollBarV(w, 0, h, this);
            EnableScissorTest = true;
        }
        public override void Draw()
        {

            Patches.Clear();
            UISys.Skin().DrawBox((int)WidX, (int)WidY, (int)WidW,(int)WidH);
            UISys.Skin().DrawBoxText((int)(WidX + WidW / 2 - UISys.Skin().SmallFont.Width(Name) / 2), (int)WidY + 8, Name);
            int oy = Scroll.Current;
            int dy = 25-oy;
            
            sy = -1;
            ey = -1;
            foreach (var i in ItemRoot.Sub)
            {
                
                dy=DrawItem(i, dy);
            }
            Scroll.Max = (dy + oy);
            Scroll.ViewH = (ey - sy);
            Scroll.Rebuild();
        }
        private float sy, ey;
        public int DrawItem(UIItem i, int y, int lc = 0)
        {
            if (y > 0 || y < WidH)
            {




                int dx = lc * 25 + 15;

                if (i.Open)
                {
                    UISys.Skin().DrawLine((int)WidX + dx - 10, (int)WidY + y + 12, (int)WidX + dx - 4, (int)WidY + y + 12, new OpenTK.Vector4(1, 0.3f, 0.3f, 1.0f));

                }
                else
                {
                    UISys.Skin().DrawLine((int)WidX + dx - 7, (int)WidY + y + 6, (int)WidX + dx - 7, (int)WidY + y + 18, new OpenTK.Vector4(1, 0.3f, 0.3f, 1.0f));
                    UISys.Skin().DrawLine((int)WidX + dx - 10, (int)WidY + y + 12, (int)WidX + dx - 4, (int)WidY + y + 12, new OpenTK.Vector4(1, 0.3f, 0.3f, 1.0f));

                }
                if ((WidY + y) > WidY)
                {
                    if (sy < 0) sy = y;

                }
                if ((WidY + y) < WidY + WidH)
                {
                    ey = y;
                }
                UISys.Skin().DrawBoxText((int)WidX + dx, (int)WidY + y, i.Name);
                UIPatch p = new UIPatch
                {
                    X = (int)WidX + dx - 8,
                    Y = (int)WidY + y + 4,
                    W = (int)WidW,
                    H = (int)25
                };
                if ((p.X + p.W) > WidX + WidW)
                {
                    p.W = (int)WidW - (p.X - (int)WidX);
                }
                p.Action = () =>
                {

                    if (i.Open)
                    {
                        i.Open = false;
                    }
                    else
                    {
                        i.Open = true;
                        Open(i);
                    }


                };
                AddPatch(p);
            }
            if (i.Open)
            {
                 y += 25;

                int ny = y;
                foreach (var si in i.Sub)
                {

                    y=DrawItem(si,y, lc + 1);
                    y += 25;
                }
            }
            return y;
        }
    }
}
