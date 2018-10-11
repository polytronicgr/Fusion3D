using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Input;
namespace Vivid3D.UI.UIWidgets
{
    public delegate void Dragged(int dx,int dy);
    public class UIDragZone : UIWidget
    {
        public bool dragging = false;
        int lx, ly;
        public int DraggedX, DraggedY;
        public Dragged DragEv = null;
        public UIDragZone(int x,int y,int w,int h,UIWidget top) : base(x,y,w,h,"",top)
        {

        }
        public override void OnMouseDown(UIMouseButton b)
        {
            DraggedX = 0;
            DraggedY = 0;
            dragging = true;
            lx = VInput.MX;
            ly = VInput.MY;
            if (DragEv != null)
            {
                Console.WriteLine("Yep");
            }
        }
        public override void OnMouseUp(UIMouseButton b)
        {
            dragging = false;
        }
        public override void Update()
        {
            if (dragging)
            {
             //   Console.WriteLine("Dragging!");
                DraggedX = VInput.MX - lx;
                DraggedY = VInput.MY - ly;
                lx = VInput.MX;
                ly = VInput.MY;
                if (DragEv != null)
                {
                    DragEv(DraggedX, DraggedY);
                }
            }
            else
            {
                DraggedX = 0;
                DraggedY = 0;
            }
        }
    }
}
