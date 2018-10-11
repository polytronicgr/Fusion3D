using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.UI;
using OpenTK;
namespace Vivid3D.UI.UIWidgets
{
    public class UIWindow : UIWidget
    {
        public Vivid3D.App.VividApp AppLink = null;
        private UIDragZone titleDrag;
        private UIDragZone sizeDrag;
        private UIDragZone rightDrag;
        private UIDragZone leftDrag;
        private UIDragZone botDrag;
        public float Alpha = 0.85f;
        public bool PosLocked = false;
        public bool SizeLocked = false;
        public bool DrawTitle = true;
        public Vector4 DragCol = new Vector4(0.4f, 0.4f, 0.4f, 0.4f); 
        public UIWindow(int x, int y, int w, int h, string title, UIWidget top=null) : base(x, y, w, h, title, top)
        {
            titleDrag = new UIDragZone(0, 0, (int)WidW, UISys.Skin().TitleHeight, this);
            sizeDrag = new UIDragZone((int)WidW - 15, (int)WidH - 15, 15, 15, this);
            botDrag = new UIDragZone(0, (int)WidH - 10, (int)WidW-15, 10, this);
            rightDrag = new UIDragZone((int)WidW - 10, UISys.Skin().TitleHeight + 1, 10, (int)WidH - 15 - UISys.Skin().TitleHeight, this);
            void RightDrag(int dx,int dy)
            {
                Resize(dx, 0);
            }
            rightDrag.DragEv = RightDrag;

            leftDrag = new UIDragZone(0, UISys.Skin().TitleHeight + 1, 10, (int)WidH - UISys.Skin().TitleHeight - 2, this);
        
            EnableScissorTest = true;
        }
        public override void Resized()
        {
            titleDrag.WidW = WidW;
            sizeDrag.LocX = WidW - 15;
            sizeDrag.LocY = WidH - 15;
            botDrag.WidW = WidW - 15;
            botDrag.LocY = WidH - 10;
            rightDrag.LocX = WidW - 10;
            rightDrag.WidH = WidH - 15 - UISys.Skin().TitleHeight;
            leftDrag.WidH = WidH - UISys.Skin().TitleHeight - 2;
            //Console.WriteLine("Resized!");
        }
        public override void Draw()
        {
            UISys.Skin().DrawWindow(this);
           // UISys.Skin().DrawRect((int)WidX + (int)WidW - 15, (int)WidY + (int)WidH - 15, 15, 15,DragCol);
          //  UISys.Skin().DrawRect((int)WidX, (int)WidY+UISys.Skin().TitleHeight + 1, 10, (int)WidH - UISys.Skin().TitleHeight - 2,DragCol);
         //   UISys.Skin().DrawRect((int)WidX, (int)WidY+(int)WidH - 10, (int)WidW - 15, 10,DragCol);
       //     UISys.Skin().DrawRect((int)WidX + (int)WidW - 10, (int)WidY + UISys.Skin().TitleHeight + 1, 10, (int)WidH - UISys.Skin().TitleHeight - 16,DragCol);
        }
        public bool Dragged = false;
        public bool Docked = false;
        public bool Undock = false;
        public void StopDrag()
        {
            titleDrag.dragging = false;
        }
        public override void OwnerResized()
        {
            if (Docked)
            {
                WidW = Top.Top.WidW;
                WidH = Top.Top.WidH;
            }
        }
        public void Dock(UIWindowDock d)
        {
            UISys.ActiveWindow = this;
            d.DockWin();
        }
        public override void Update()
        {
         
            if(Docked == true)
            {
           
                
            }
            else
            {
                PosLocked = false;
            }
            //Console.WriteLine("X:" + titleDrag.DraggedX + " Y:" + titleDrag.DraggedY);
            Dragged = false;
            if (PosLocked == false)
            {

                if (titleDrag.DraggedX != 0 || titleDrag.DraggedY != 0)
                {
                    Dragged = true;
                    if(Docked == true)
                    {
                        Undock = true;
                       
                     
                    }
                }
              
                if (Docked == false)
                {
                   
                    Move(titleDrag.DraggedX, titleDrag.DraggedY);
                }
                    titleDrag.DraggedX = 0;
                titleDrag.DraggedY = 0;
            }


            if (SizeLocked == false)
            {
             //   Console.WriteLine("Drag:" + rightDrag.DraggedX);
                    Resize(sizeDrag.DraggedX, sizeDrag.DraggedY);
                    Resize(rightDrag.DraggedX, 0);
                    Resize(0, botDrag.DraggedY);
                    Move(leftDrag.DraggedX, 0);
                    Resize(-leftDrag.DraggedX, 0);
            
            }
        }
    }
}
