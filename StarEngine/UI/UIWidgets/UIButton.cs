using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.UI;
using Vivid3D.Input;
namespace Vivid3D.UI.UIWidgets
{
    public delegate void Drag(int x,int y);
    public delegate void Clicked();
    public class UIButton : UIWidget
    {
        public Clicked Click;
        public ButState State = ButState.Norm;
 
        public EventHandler Enter;
        public EventHandler Leave;
        public EventHandler Pressed;
        public Drag Dragged;
        public UIButton(float x, float y, float w, float h, string text, UIWidget root) : base(x, y, w, h, text, root)
        {

        }
        public override void OnEnter()
        {
            if (Enter != null) Enter(this, null);
            State = ButState.Hover;
        }
        public override void OnLeave()
        {
            if (Leave != null) Leave(this, null);
            State = ButState.Norm;
        }
        public override void OnMouseDown(UIMouseButton b)
        {
            lx = VInput.MX;
            ly = VInput.MY;
            State = ButState.Press;
            UISys.Skin().ClickSound();
       
        }
        public override void OnMouseUp(UIMouseButton b)
        {

            if (Click != null)
            {
                Click();
            }
            State = ButState.Norm;
            Event(new UIEvent(this, EventType.Click));
            lx = 0;
            ly = 0;
        }
        public override void OnActivate()
        {
        
        }
        public override void OnDeactivate()
        {
         
        }
        int lx, ly;
        public override void Draw()
        {
            if (State == ButState.Press)
            {
                if (Dragged != null)
                {
                    Dragged(VInput.MX - lx, VInput.MY - ly);
                    lx = VInput.MX;
                    ly = VInput.MY;
                }
            }
            UISys.Skin().DrawButton(this);

        }

    }
}
