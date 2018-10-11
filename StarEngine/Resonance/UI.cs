using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Font;
using Vivid3D.Input;
using Vivid3D.Logic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.Resonance
{
    public class UI
    {

        public static UIForm TopForm = null;
        public Logics Logics = new Logics();
        public Logics Graphics = new Logics();

        public static Font.VFont Font = null;
        public static UIForm Active = null;
        public UIForm Top = null;

        public UIForm Root;

        public UI()
        {
            InitUI();
            for(int i = 0; i < 32; i++)
            {
                Pressed[i] = null;
            }
        }
        private Texture.VTex2D Black = null;
        public void InitUI()
        {
            Black = new Texture.VTex2D("data\\ui\\black.png", Texture.LoadMethod.Single, false);
            Font = new VFont("data/font/times.ttf.vf");
        }

        public bool FirstMouse = true;
        public static int MX,MY,MXD,MYD;

        public void Update()
        {

            Logics.SmartUpdate();

            if (FirstMouse)
            {
                MX = VInput.MX;
                MY = VInput.MY;
                FirstMouse = false;
            }

            MXD = VInput.MX - MX;
            MYD = VInput.MY - MY;
            MX = VInput.MX;
            MY = VInput.MY;

            if (Top != null)
            {
                UpdateUpdateList(Top);
            }
            else
            {
                UpdateUpdateList(Root);
            }
            foreach(var form in UpdateList)
            {
                form.Update?.Invoke();
            }
            int f = 0;
            var top = GetTopForm(MX, MY);

            if (top != null)
            {

                if (TopForm != top)
                {
                    if (TopForm != null)
                    {

                        if (TopForm != Pressed[0])
                        {
                            TopForm.MouseLeave?.Invoke();
                        }
                    }


                    top.MouseEnter?.Invoke();

                }


            }
            if (top != null)
            {
                if (top == TopForm)
                {

                    top.MouseMove?.Invoke(MX - top.GX, MY - top.GY, MXD, MYD);

                }
            }
            if (top == null)
            {
                if (TopForm != null)
                {
                    if (TopForm != Pressed[0])
                    {
                        TopForm.MouseLeave?.Invoke();
                    }
                }
            }
            TopForm = top;

            if (Active != null)
            {

                var key = VInput.KeyIn();
                if (key != OpenTK.Input.Key.LastKey)
                {
                    if (key == OpenTK.Input.Key.LastKey)
                    {
                        LastKey = OpenTK.Input.Key.LastKey;
                        NextKey = 0;
                    }
                    if (key == LastKey)
                    {
                        bool shift = false;
                        if (VInput.KeyIn(OpenTK.Input.Key.ShiftLeft))
                        {
                            shift = true;
                        }
                        if (VInput.KeyIn(OpenTK.Input.Key.ShiftRight))
                        {
                            shift = true;
                        }
                        if (Environment.TickCount > NextKey)
                        {
                            Active.KeyPress?.Invoke(key,shift);
                            NextKey = Environment.TickCount + 90;
                        }
                    }
                    else
                    {
                        bool shift = false;
                        if (VInput.KeyIn(OpenTK.Input.Key.ShiftLeft))
                        {
                            shift = true;
                        }
                        if (VInput.KeyIn(OpenTK.Input.Key.ShiftRight))
                        {
                            shift = true;
                        }
                        LastKey = key;
                        Active.KeyPress?.Invoke(key,shift);
                        NextKey = Environment.TickCount + 250;
                    }
                }
            }

            if (VInput.MB[0])
            {

                if (TopForm != null)
                {
                    if (Pressed[0] == null)
                    {
                        Console.WriteLine("Click:" + clicks);
                        if(Environment.TickCount<lastClick+300)
                        {
                            clicks++;
                            if (clicks == 2)
                            {

                                TopForm.DoubleClick?.Invoke(0);
                                Console.WriteLine("DoubleClicked:"+TopForm.Text+":"+TopForm.GetType().ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Click One");
                            clicks = 1;
                        }
                        lastClick = Environment.TickCount;
                        TopForm.MouseDown?.Invoke(0);
                        Pressed[0] = TopForm;
                        if (Active != TopForm)
                        {
                            if (Active != null)
                            {
                                Active.Deactivate?.Invoke();
                            }
                        }
                        Active = TopForm;
                        TopForm.Activate?.Invoke();
                        
                        if (sdrag)
                        {
                            sdx = MX;
                            sdy = MY;
                    
                        
                        }
                    }
                    else if (Pressed[0] == TopForm)
                    {
                        TopForm.MousePressed?.Invoke(0);
                    }
                    else if (Pressed[0] != TopForm)
                    {
                        Pressed[0].MousePressed?.Invoke(0);

                    }
                }
                else
                {
                    if (Pressed[0] != null)
                    {
                        Pressed[0].MousePressed?.Invoke(0);
                    }
                }

                if (Pressed[0] != null)
                {

                 //   Console.WriteLine("MX:" + MX + " MY:" + MY + " SDX:" + sdx + " SDY:" + sdy);
                    int mvx = MX - sdx;
                    int mvy = MY - sdy;
                    if (mvx != 0 || mvy != 0)
                    {
                       
                        Pressed[0].Drag?.Invoke(mvx, mvy);
                        Pressed[0].PostDrag?.Invoke(mvx,mvy);
                    }
                        sdx = MX;

                    sdy = MY;
                    //Console.WriteLine(@)

                    //sdx = MX-Pressed[0].GY;

                    //sdy = MY-Pressed[0].GY;
                }

            }
            else
            {
                //Console.WriteLine("Wop");
                if (Pressed[0] != null)
                {
                    if (Pressed[0].InBounds(MX, MY) == false)
                    {
                        Pressed[0].MouseLeave?.Invoke();
                    }
        
                    Pressed[0].MouseUp?.Invoke(0);
                    Pressed[0] = null;
                    sdrag = true;

                }
            }


        }
        public int NextKey = 0;
        public OpenTK.Input.Key LastKey = OpenTK.Input.Key.LastKey;
        public int clicks = 0;
        public int lastClick = 0;
        private bool sdrag = true;
        private int sdx, sdy, ux, uy;
        public UIForm[] Pressed = new UIForm[32];
        private UIForm GetTopForm(int mx, int my)
        {
            foreach (var form in UpdateList)
            {
                if (form.CheckBounds == true)
                {

                    if (form.InBounds(mx, my))
                    {
                        //    Console.WriteLine("Form:" + form.Text);
                        return form;

                    }
                }

            }
            return null;
        }
        private float TopB = 0.0f;
        public void Render()
        {
            if (Top != null)
            {
                TopB = TopB + 0.065f;
                if (TopB > 0.8f)
                {
                    TopB = 0.8f;
                }
            }
            else
            {
                TopB = TopB - 0.085f;
                if (TopB < 0) TopB = 0;
            }
            Graphics.SmartUpdate();

            if (Top != null)
            {
                UpdateRenderList(Root);
                foreach(var form in RenderList)
                {
                    form.Draw?.Invoke();
                }
                var ntex = new Texture.VTex2D(Vivid3D.App.VividApp.W, Vivid3D.App.VividApp.H);

                ntex.CopyTex(0, 0);
                OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit);
                Vivid3D.Draw.VPen.RectBlur2(0, 0, Vivid3D.App.VividApp.W, Vivid3D.App.VividApp.H, ntex, new OpenTK.Vector4(1, 1, 1, 1), TopB);


                UpdateRenderList(Top);

                foreach (var form in RenderList)
                {
                   
                    form.Draw?.Invoke();
                }
              
                ntex.Delete();
            }
            else
            {
                UpdateRenderList(Root);


                foreach (var form in RenderList)
                {

                            
                        form.Draw?.Invoke();
                    

                }
                if (TopB > 0)
                {
                    Texture.VTex2D ntex = new Texture.VTex2D(Vivid3D.App.VividApp.W, Vivid3D.App.VividApp.H);
                    ntex.CopyTex(0, 0);
                    OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit);
                    Vivid3D.Draw.VPen.RectBlur2(0, 0, Vivid3D.App.VividApp.W, Vivid3D.App.VividApp.H, ntex, new OpenTK.Vector4(1, 1, 1, 1), TopB);
                    ntex.Delete();
                }
            }
        }

        public List<UIForm> UpdateList = new List<UIForm>();
        public List<UIForm> RenderList = new List<UIForm>();

        private void UpdateUpdateList(UIForm begin)
        {
            UpdateList.Clear();

            AddNodeBackward(UpdateList, begin);



        }

        private void UpdateRenderList(UIForm begin)
        {

            RenderList.Clear();

            AddNodeForward(RenderList, begin);
            

        }

        private void AddNodeBackward(List<UIForm> forms,UIForm form)
        {
            int fc = form.Forms.Count;
            if (fc > 0)
            {
                while (true)
                {
                    fc--;
                    var af = form.Forms[fc];
                    AddNodeBackward(forms, af);
                    if (fc == 0) break;
                }
            }
            forms.Add(form);

        }

        private void AddNodeForward(List<UIForm> forms,UIForm form)
        {

            RenderList.Add(form);
            foreach(var nf in form.Forms)
            {
                AddNodeForward(forms, nf);
            }

        }
            

    }


}
