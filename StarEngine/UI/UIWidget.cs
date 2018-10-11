using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.UI
{
    public enum DockMode
    {
        Left,Right,Top,Bot,Center,None
    }
    public class UIWidget
    {
        public UISys Owner = null;
        public UIWidget Top;
        public DockMode Docking = DockMode.None;
        public float AR = 1.0f;
        public UIItem ItemRoot = new UIItem();
        public UIWidgets.UIWindow ActiveWindow = null;
        public UISys GetOwn
        {
            get
            {
                if (Owner != null) return Owner;
                if (Top != null)
                {
                    return Top.GetOwn;
                }
                return null;
            }
        }
        public List<UIWidget> Sub = new List<UIWidget>();
        public float WidX
        {
            get
            {
                if (Top == null) return _WidX * Vivid3D.App.AppInfo.RW;
                return Top.WidX + _WidX * Vivid3D.App.AppInfo.RW;
            }
            set
            {
                _WidX = value / Vivid3D.App.AppInfo.RW;
            }
        }
        public float WidY
        {
            get
            {
                if (Top == null) return _WidY * Vivid3D.App.AppInfo.RH;
                return Top.WidY + _WidY * Vivid3D.App.AppInfo.RH;
            }
            set
            {
                _WidY = value / (float)Vivid3D.App.AppInfo.RH;
            }
        }
        public float LocX
        {
            set
            {
                _WidX = value;
            }
        }
        public float LocY
        {
            set
            {
                _WidY = value;
            }
        }
        public float WidW
        {
            get
            {
                return _WidW * Vivid3D.App.AppInfo.RW;
            }
            set
            {
                _WidW = value / App.AppInfo.W;

                
            }
        }
        public float WidH
        {
            get
            {
                return _WidH * Vivid3D.App.AppInfo.H;
            }
            set
            {
                _WidH = value / App.AppInfo.H;

            }
        }
        public bool WidOpen = true;
        public void Close()
        {
            WidOpen = false;
        }
        public float _WidW = 0, _WidH = 0;
        public float _WidX = 0, _WidY = 0;
        public string Name = "";
        public List<String> Text = new List<string>();
        public Dictionary<string, UIWidget> WidMap = new Dictionary<string, UIWidget>();
        public bool EnableScissorTest = false;
        public bool NonActive = false;
        public UIItem AddItem(UIItem i)
        {
            ItemRoot.Add(i);
            return i;
        }
        public void AddWidget(UIWidget w)
        {
            Sub.Add(w);
            w.Top = this;
        }
        public UIWidget(float x, float y, float w = 0, float h = 0, string text = "", UIWidget top = null)
        {
            if(top!=null)
            {
                top.AddWidget(this);
            }

            WidX = x;
            WidY = y;
            WidW = w;
            WidH = h;
           
            Name = text;
        
        }
        public virtual void OwnerResized()
        {

        }
        public virtual void OnOwnerResized(bool first=true)
        {
         
            OwnerResized();
            foreach (var w in Sub)
            {
                w.OnOwnerResized(false);
            }   
        }
        public virtual void OnEnter()
        {

        }
        public virtual void OnLeave()
        {

        }
        public virtual void OnHover()
        {

        }
        public virtual void OnMouseDown(UIMouseButton b)
        {

        }
        public virtual void OnMouseUp(UIMouseButton b)
        {

        }
        public virtual void OnActivate()
        {

        }
        public virtual void OnDeactivate()
        {

        }
        public virtual void OnActive()
        {

        }
        public virtual bool InBounds()
        {
            if (VInput.MX >= WidX)
            {
                if (VInput.MY >= WidY)
                {
                    if (VInput.MX <= WidX + WidW)
                    {
                        if (VInput.MY <= WidY + WidH)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public virtual bool OnUpdate()
        {
            if (this.WidOpen == false) return false; 
           for(int wi = Sub.Count - 1; wi > -1; wi--)
            {
                UIWidget w = Sub[wi];
               
                    if (w.OnUpdate())
                    {
                        return true;
                    }
               
            }
            if (UISys.Active != null)
            {
             
                if(UISys.Active is UIWidgets.UIDragZone)
                {
                    UISys.ActiveWindow = UISys.Active.Top as UIWidgets.UIWindow;
                  
                }
            }
            
                if (UISys.Active == null)
            {
                UISys.IsKeyIn = false;
            }

            if (UISys.Active == this)
            {
                if (VInput.AnyKey())
                {
                    if (UISys.IsKeyIn == false)
                    {
                        UISys.IsKeyIn = true;
                        UISys.KeyIn = VInput.KeyIn();
                        UISys.LastStroke = Environment.TickCount;
                        UISys.NextStroke = UISys.LastStroke + UISys.FirstStrokeWait;
                        UISys.Active.KeyIn(UISys.KeyIn, VInput.IsShiftIn());

                        if (UISys.KeyIn == OpenTK.Input.Key.BackSpace)
                        {
                            this.KeyBackSpace();
                        }
                        else if (UISys.KeyIn == OpenTK.Input.Key.Tab)
                        {
                            if(UISys.Active == this)
                            {
                                int ci = 0;
                                foreach(var w in UISys.Active.Top.Sub)
                                {
                                    if (w == UISys.Active) break;
                                    ci++;
                                }
                                UISys.Active.OnDeactivate();
                                if(ci<UISys.Active.Top.Sub.Count-1)
                                {
                                    UISys.Active = UISys.Active.Top.Sub[ci + 1];
                                    UISys.Active.OnActivate();
                                }
                                else
                                {
                                    UISys.Active = UISys.Active.Top.Sub[0];
                                    UISys.Active.OnActivate();
                                }
                            }
                        }
                        else if (UISys.KeyIn == OpenTK.Input.Key.Delete)
                        {
                            this.KeyDel();
                        }
                        else if (UISys.KeyIn == OpenTK.Input.Key.Left)
                        {
                            this.KeyLeft();
                        }
                        else if (UISys.KeyIn == OpenTK.Input.Key.Right)
                        {
                            this.KeyRight();
                        }
                        else
                        {
                            this.KeyAdd(UISys.KeyIn, VInput.IsShiftIn());
                        }
                    }
                    else
                    {
                        if (VInput.KeyIn() == UISys.KeyIn)
                        {
                            if (Environment.TickCount >= (UISys.NextStroke))
                            {
                                UISys.LastStroke = Environment.TickCount;
                                UISys.NextStroke = UISys.LastStroke + UISys.NextStrokeWait;
                                if (UISys.KeyIn == OpenTK.Input.Key.BackSpace)
                                {
                                    KeyBackSpace();
                                }
                                else if (UISys.KeyIn == OpenTK.Input.Key.Delete)
                                {
                                    this.KeyDel();
                                }
                                else if (UISys.KeyIn == OpenTK.Input.Key.Left)
                                {
                                    this.KeyLeft();
                                }
                                else if (UISys.KeyIn == OpenTK.Input.Key.Right)
                                {
                                    this.KeyRight();
                                }
                                else
                                {
                                    if (VInput.IsShiftIn())
                                    {
                                        KeyAdd(UISys.KeyIn, true);
                                    }
                                    else
                                    {
                                        KeyAdd(UISys.KeyIn, false);
                                    }
                                }
                            }
                        }
                        else
                        {

                            this.KeyUp(UISys.KeyIn, VInput.IsShiftIn());
                            UISys.IsKeyIn = true;
                            UISys.KeyIn = VInput.KeyIn();
                            UISys.LastStroke = Environment.TickCount;
                            UISys.NextStroke = UISys.LastStroke + UISys.FirstStrokeWait;
                            UISys.Active.KeyIn(UISys.KeyIn, VInput.IsShiftIn());
                            if (UISys.KeyIn == OpenTK.Input.Key.BackSpace)
                            {
                                KeyDel();
                            }
                            else if (UISys.KeyIn == OpenTK.Input.Key.Left)
                            {
                                this.KeyLeft();
                            }
                            else if (UISys.KeyIn == OpenTK.Input.Key.Right)
                            {
                                this.KeyRight();
                            }
                            else
                            {
                                if (VInput.IsShiftIn())
                                {
                                    KeyAdd(UISys.KeyIn, true);
                                }
                                else
                                {
                                    KeyAdd(UISys.KeyIn, false);
                                }
                            }
                        }
                    }
                }
                else
                {
                    UISys.IsKeyIn = false;
                }
            }
            if (InBounds())
            {
                if (UISys.Over == this)
                {
                    this.OnHover();
                }
                if (UISys.Over == null)
                {
                    UISys.Over = this;
                    this.OnEnter();
                }
                if (UISys.Over != null && UISys.Over != this)
                {
                    UISys.Over.OnLeave();
                    UISys.Over = this;
                    UISys.Over.OnEnter();
                }

            }
            else
            {
                if (UISys.Over == this && UISys.Pressed != this)
                {
                    UISys.Over.OnLeave();
                    UISys.Over = null;
                }
            }
            if (VInput.MB[0] == false && UISys.Lock == true)
            {
                UISys.Lock = false;
            }
            if (VInput.MB[0] && UISys.Lock == false)
            {

                if (UISys.Over != null)
                {
                    if (UISys.Over == this)
                    {
                        if (UISys.Active != this)
                        {
                            if (UISys.Active != null)
                            {
                                if (UISys.IsKeyIn)
                                {
                                    UISys.Active.KeyUp(UISys.KeyIn, VInput.IsShiftIn());
                                    UISys.IsKeyIn = false;
                                }
                                UISys.Active.OnDeactivate();
                            }
                            UISys.IsKeyIn = false;

                            UISys.Active = this;
                            this.OnActivate();
                            UISys.Lock = true;
                            foreach(var p in Patches)
                            {
                                if(VInput.MX>=p.X)
                                {
                                    if (VInput.MY >= p.Y)
                                    {
                                        if (VInput.MX <= (p.X + p.W))
                                        {
                                            if (VInput.MY <= (p.Y + p.H))
                                            {
                                              
                                                p.Action();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                }

            }
            if (InBounds())
            {
                if (VInput.MB[0])
                {

                    if (UISys.Pressed == null)
                    {
               
                        OnMouseDown(UIMouseButton.Left);
                        UISys.Pressed = this;
                        foreach (var p in Patches)
                        {
                            if (VInput.MX >= p.X)
                            {
                                if (VInput.MY >= p.Y)
                                {
                                    if (VInput.MX <= (p.X + p.W))
                                    {
                                        if (VInput.MY <= (p.Y + p.H))
                                        {
                                          
                                            p.Action();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (UISys.Pressed == this)
                    {
                     
                        OnMouseUp(UIMouseButton.Left);
                        UISys.Pressed = null;
                    }
                }
            }
            else
            {
                if (VInput.MB[0] == false)
                {
                    if (UISys.Pressed == this)
                    {
                    
                        UISys.Pressed.OnMouseUp(UIMouseButton.Left);
                        UISys.Pressed = null;
                    }
                }
                if (UISys.Pressed != null)
                {
                    // UISys.Pressed.OnMouseUp(UIMouseButton.Left);
                    //UISys.Pressed = null;
                }
            }
            //this.Update();
            if (UISys.Over == this || UISys.Pressed == this)
            {
                return true;
            }
            return false;
        }
        public virtual void Event(UIEvent e)
        {
            OnEvent(e);
            if (Top != null)
            {
                Top.Event(e);
            }
        }
        public virtual void AppResize(int w,int h)
        {

        }
        public virtual void OnAppResize(int w,int h)
        {
            AppResize(w, h);
            foreach(var s in Sub)
            {
                s.OnAppResize(w, h);
            }
        }
        public virtual void OnEvent(UIEvent e)
        {

        }
        public virtual void KeyIn(OpenTK.Input.Key k,bool shift)
        {

        }
        public virtual void KeyUp(OpenTK.Input.Key k,bool shift)
        {

        }
        public virtual void KeyLeft()
        {

        }
        public virtual void KeyRight()
        {

        }
        public virtual void KeyDel()
        {
        }
        public virtual void KeyAdd(OpenTK.Input.Key k,bool shift)
        {

        }
        public virtual void KeyBackSpace()
        {

        }
        public virtual void KeyEnter()
        {
            
        }
        public virtual void Move(int x,int y)
        {
            if(this is UIWidgets.UIWindow)
            {
                var w = this as UIWidgets.UIWindow;
            //    if (w.Docked) return;
            }
            if (x != 0)
            {
                WidX = WidX + x;
            }
            if (y != 0)
            {
                WidY = WidY + y;
            }
        }
        public virtual void Resized()
        {

        }
        public virtual void Resize(int x,int y)
        {
            WidW = WidW + x;
            WidH = WidH + y;
            Resized();
        }
        public virtual void ChangeSize(int w,int h)
        {
            WidW = w;
            WidH = h;
            Resized();
        }
        public List<Vivid3D.UI.UIWidgets.UIPatch> Patches = new List<Vivid3D.UI.UIWidgets.UIPatch>();
        public void AddPatch(Vivid3D.UI.UIWidgets.UIPatch p)
        {
            Patches.Add(p);
        }
        public virtual void OnDraw()
        {
            if (this.WidOpen == false) return;
            if (EnableScissorTest)
            {
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor((int)WidX, Vivid3D.App.AppInfo.H - ((int)WidY + (int)WidH), (int)WidW, (int)WidH);
                GL.Viewport((int)WidX, (int)WidY, (int)WidW, (int)WidH);

            }
                this.Draw();
            GL.Disable(EnableCap.ScissorTest);
            if (EnableScissorTest)
            {
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor((int)WidX,Vivid3D.App.AppInfo.H-((int)WidY+(int)WidH), (int)WidW, (int)WidH);
                GL.Viewport((int)WidX, (int)WidY, (int)WidW, (int)WidH);

                foreach (var w in Sub)
                {
                   
                    w.OnDraw();
                }
                GL.Disable(EnableCap.ScissorTest);
            }
            else
            {
                foreach(var w in Sub)
                {
                    w.OnDraw();
                }
            }
        }
        public virtual void UpdateAll()
        {
            Update();
            foreach(var w in Sub)
            {
                w.UpdateAll();
            }
        }
        public virtual void Update()
        {

        }
        public virtual void Draw()
        {

        }

    }
}
