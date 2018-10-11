using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Font;
using Vivid3D.Visuals;
using Vivid3D.Draw;
namespace Vivid3D.UI.UIWidgets
{
    public class UIWindowDock : UIWidget
    {
        public bool WinOver = false;
        public UIWindowDock(string type,UIWidget root=null) : base(0,0,root.WidW,root.WidH,type,root)
        {
           

        }
        public override void Update()
        {
            if(UISys.ActiveWindow != null)
            {
                var w = UISys.ActiveWindow;
                if (w.WidX >= WidX && w.WidX <= WidX + WidW)
                {
                    if (w.WidY > WidY && w.WidY < WidY + 50)
                    {
                        WinOver = true;
                    }
                    else
                    {
                        WinOver = false;
                    }
                }
                else
                {
                    WinOver = false;
                }
            }    

            
        }
        public override void OwnerResized()
        {
            WidW = Top.WidW;
        }
        public override void Draw()
        {

            VFontRenderer.Draw(UISys.Skin().SmallFont, Name, WidX + 5, WidY + 5, new OpenTK.Vector4(1, 1, 1, 1));
            if (WinOver == false)
            {
                VPen.Rect(WidX, WidY, WidW, 50, new OpenTK.Vector4(0, 0, 0.5f, 0.1f));
            }
            else
            {
                VPen.Rect(WidX, WidY, WidW, 50, new OpenTK.Vector4(0, 0, 0.5f, 0.35f));
                if (Vivid3D.Input.VInput.MB[0] == false && UISys.ActiveWindow.Docked == false)
                {
                    DockWin();
                }
            }
        }

        public void DockWin()
        {
            UISys.ActiveWindow.Docked = true;


            if (UISys.Active != null) UISys.Active.OnDeactivate();
            UISys.Active = null;

            if (UISys.Over != null) UISys.Over.OnLeave();
            UISys.Over = null;

            UISys.Lock = false;

            UISys.Pressed = null;
            UISys.ActiveWindow.Top.Sub.Remove(UISys.ActiveWindow);
            UISys.ActiveWindow.StopDrag();
            UISys.ActiveWindow.Docked = true;
            UISys.ActiveWindow.WidX = 0;
            UISys.ActiveWindow.WidY = 0;
            UISys.ActiveWindow.WidW = Top.WidW;
            UISys.ActiveWindow.WidH = Top.WidH;
            UISys.ActiveWindow.Top = this;
            UISys.ActiveWindow.Top.Sub.Add(UISys.ActiveWindow);
        }
    }
}
