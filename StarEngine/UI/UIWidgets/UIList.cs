using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Font;

namespace Vivid3D.UI.UIWidgets
{
    public delegate void SelectNode(UIItem i);
    public class UIList: UIWidget
    {
        public UIScrollBarV Scroll;
        public SelectNode Select= null;
        public UIList(int x, int y, int w, int h, string title, UIWidget root = null) : base(x, y, w, h, title, root)
        {
            Scroll = new UIScrollBarV(w, 0, h, this);
            EnableScissorTest = true;
        }
        public override void Draw()
        {

            Patches.Clear();
            UISys.Skin().DrawBox((int)WidX, (int)WidY, (int)WidW, (int)WidH);
            UISys.Skin().DrawBoxText((int)(WidX + WidW / 2 - UISys.Skin().SmallFont.Width(Name) / 2), (int)WidY + 8, Name);
            int oy = Scroll.Current;
            int dy = 30 - oy;
         
            sy = -1;
            ey = -1;
            foreach (var i in ItemRoot.Sub)
            {

                dy = DrawItem(i, dy);
            }
            Scroll.Max = (dy + oy);
            Scroll.ViewH = (ey - sy);
            Scroll.Rebuild();
        }
        private float sy, ey;
        public int DrawItem(UIItem i, int y, int lc = 0)
        {
            int dx = lc * 25 + 15;

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
                H = (int)25,
                Action = () =>
                {

                    Select(i);

                }
            };
            AddPatch(p);


            return y + 25;
        }
    }
}
