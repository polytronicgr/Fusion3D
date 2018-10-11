using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI.UIWidgets
{
    
    public class UISplitterV : UIWidget
    {
        public SizeChange SizeAction;
        public UIPanel Top, Bot;
        public UIButton Mover;
        public int SplitY = 0;
        public UISplitterV(int x, int y, int w, int h, UIWidget top = null) : base(x, y, w, h, "", top)
        {
            SplitY = h / 2;
            Top = new UIPanel(0,0, w,SplitY-5, "Top", this);
            Bot = new UIPanel(0, SplitY+5, w, h/2, "Bot", this);
            Mover = new UIButton(0,SplitY - 5, w, 10, "*", this);

            top.Docking = DockMode.Center;
            Bot.Docking = DockMode.Center;
            Mover.Dragged = (ax, ay) =>
            {
                SplitY += ay;
                Mover.WidY = SplitY;
                Top.WidH = SplitY - 5;
                Bot.WidH = h / 2 - 10;
                Bot.WidY = SplitY + 5;
                Bot.WidH = (WidH- SplitY);
                if (SizeAction != null)
                {
                    SizeAction();
                }
                foreach(var a in Top.Sub)
                {
                    a.OnOwnerResized();
                }
                foreach(var a in Bot.Sub)
                {
                    a.OnOwnerResized();
                }
            };
        }
        public void Split(int y)
        {
            SplitY = y;
            Mover.WidY = SplitY;
            Top.WidH = SplitY - 5;
            Bot.WidH = WidH / 2 - 10;
            Bot.WidY = SplitY + 5;
            Bot.WidH = (WidH - SplitY);
            if (SizeAction != null)
            {
                SizeAction();
            }
            foreach (var a in Top.Sub)
            {
                a.OnOwnerResized();
            }
            foreach (var a in Bot.Sub)
            {
                a.OnOwnerResized();
            }
        }
        public override void OwnerResized()
        {
            //Mover.WidW = Top.Top.WidW;

            Mover.WidW = Top.Top.Top.WidW;
            Top.WidW = Top.Top.Top.WidW;
            Bot.WidW = Top.Top.Top.WidW;
            //Console.WriteLine("MW:" + Mover.WidW + " W:" + WidW + " TW:" + Top.WidW + " TTW:" + Top.Top.Top.WidW);
           
        }
        public override void Draw()
        {
            // UISys.Skin().DrawRect((int)WidX + SplitX - 5, (int)WidY, 10, (int)WidH, new OpenTK.Vector4(0.8f, 0.8f, 0.8f, 1.0f));

        }
    }
}
