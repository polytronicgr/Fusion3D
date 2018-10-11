using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Vivid3D.Draw;
using Vivid3D.Import;
using OpenTK.Input;
using Vivid3D.Input;
using Vivid3D.State;
namespace Vivid3D.App
{
    public static class AppInfo
    {
        public static int W, H;
        public static bool Full;
        public static string App;
        public static int RW, RH;
    }
    public class VForm : VividApp
    {
        public void SetSize(int w,int h)
        {
            AppInfo.W = w;
            AppInfo.H = h;
            AppInfo.RW = w;
            AppInfo.RH = h;
            GL.Viewport(0, 0,w,h);
            GL.Scissor(0, 0, w, h);
            VPen.SetProj(0, 0, w, h);
        }
         public void Set(int w,int h)
        {
            GL.ClearColor(System.Drawing.Color.AliceBlue);
            GL.Enable(EnableCap.DepthTest);
            AppInfo.W = w;
            AppInfo.H = h;
            AppInfo.RW = w;
            AppInfo.RH = h;
            AppInfo.Full = false;
            AppInfo.App = "GLApp";
            Import.Import.RegDefaults();
          
            GL.Viewport(0, 0, w, h);
            GL.Scissor(0, 0,w,h);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ScissorTest);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthRange(0, 1);

            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Less);
           // UI.UISys.ActiveUI.OnResize(Width, Height);
            VPen.SetProj(0, 0, w, h);
        }

    }
    public class VividApp : GameWindow
    {
        public static VividApp Link = null;
        private string _Title = "";
        private OpenTK.Graphics.Color4 _BgCol = OpenTK.Graphics.Color4.Black;
        public static int W, H;
        public static int RW, RH;

        public static VividState InitState = null;
        
        public static Stack<VividState> States = new Stack<VividState>();

        public static VividState ActiveState
        {
            get
            {
                if (States.Count == 0) return null;
                return States.Peek();
            }
        }

        public VividApp()
        {

        }

        public static void PushState(VividState state,bool start = true)
        {

            States.Push(state);
            state.InitState();
            state.Running = true;
            state.StartState();
            
        }
  
        public static void PopState()
        {

            var ls = States.Pop();
            ls.StopState();
            ls.Running = false;

        }
      


        public OpenTK.Graphics.Color4 BgCol
        {
            get { return _BgCol; }
            set { _BgCol = value; }
        }
        public string AppName
        {
            get { return _Title; }
            set { _Title = value;Title = value; }
        }
        public void MakeFixed()
        {
            WindowBorder = WindowBorder.Fixed;
        }
        public void MakeWindowless()
        {
            this.WindowBorder = WindowBorder.Hidden;
        }
        public void MakeFullscreen()
        {
            this.WindowState = WindowState.Fullscreen;
        }
        public VividApp(string app, int width, int height, bool full) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, app, full ? GameWindowFlags.Fullscreen : GameWindowFlags.Default, DisplayDevice.Default, 4, 5, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)

        {
            Link = this;
            _Title = app;
            AppInfo.W = width;
            AppInfo.H = height;
            AppInfo.RW = width;
            AppInfo.RH = height;
            AppInfo.Full = full;
            AppInfo.App = app;
            W = width;
            H = height;
            RW = width;
            RH = height;
            Import.Import.RegDefaults();
            VPen.InitDraw();
            Vivid3D.Sound.StarSoundSys.Init();
   
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            int bid = 0;
            bid = GetBID(e);
            VInput.MB[bid] = true;
         
        }

        private static int GetBID(MouseButtonEventArgs e)
        {
            int bid = 0;
            switch (e.Button)
            {
                case MouseButton.Left:
                    bid = 0;
                    break;
                case MouseButton.Right:
                    bid = 1;
                    break;
                case MouseButton.Middle:
                    bid = 2;
                    break;
            }

            return bid;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            int bid = GetBID(e);
            VInput.MB[bid] = false;
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            VInput.MX = e.Position.X;
            VInput.MY = e.Position.Y;
          
      

        }
        bool fs = true;
        MouseState lm;
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            VInput.SetKey(e.Key, true);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            VInput.SetKey(e.Key, false);
        }
        protected override void OnResize(EventArgs e)
        {
            SetGL();
        }

        private void SetGL()
        {
            AppInfo.W = Width;
            AppInfo.H = Height;
            AppInfo.RW = Width;
            AppInfo.RH = Height;
            GL.Viewport(0, 0, Width, Height);
            GL.Scissor(0, 0, Width, Height);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ScissorTest);
            //  GL.Disable(EnableCap.Lighting);

            //GL.DepthFunc(DepthFunction.Greater);


            GL.Enable(EnableCap.DepthTest);
            GL.DepthRange(0, 1);

            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);
            if (UI.UISys.ActiveUI != null)
            {
                UI.UISys.ActiveUI.OnResize(Width, Height);
                // GL.DepthFunc(DepthFunction.Lequal);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
            VPen.SetProj(0, 0, Width, Height);
            SetGL();
            InitApp();
           
        }
        public virtual void InitApp()
        {

        }
        public virtual void UpdateApp()
        {

        }
        public virtual void DrawApp()
        {

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (InitState != null)
            {
               
                PushState(InitState);
                InitState = null;
            }
                UpdateApp();
            if (States.Count > 0)
            {
                var us = States.Peek();
                us.UpdateState();
                us.InternalUpdate();
            }
        }
        public int fpsL=0, fps=0, frames=0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            if (Environment.TickCount>fpsL+1000)
            {
                fpsL = Environment.TickCount + 1000;
                fps = frames;
                frames = 0;
    
            }
            frames++;
            Title = AppName;
            Title += $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
        
            GL.ClearColor(_BgCol);

           // GL.DepthMask(true);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawApp();
            if (States.Count > 0)
            {

                var rs = States.Peek();
                rs.DrawState();

            }
       

            SwapBuffers();
        }


    }
}
