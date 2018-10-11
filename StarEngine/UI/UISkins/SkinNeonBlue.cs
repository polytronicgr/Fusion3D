using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.UI;
using Vivid3D.Texture;
using Vivid3D.Draw;
using Vivid3D.App;
using Vivid3D.UI.UIWidgets;
using Vivid3D.Font;
using OpenTK;
namespace Vivid3D.UI.UISkins
{
    public class SkinNeonBlue : UISkin 
    {
        private int titleHeight = 25;
        public override int TitleHeight
        {
            get
            {
                return titleHeight;
            }
        }

        public SkinNeonBlue()
        {

        }
        public override void InitSkin()
        {
            Console.WriteLine("Skin!");
            PanelBG = new VTex2D("data/ui/skin/neonblue/panelbg1.png", LoadMethod.Single);
            But_Norm = new VTex2D("data/ui/skin/neonblue/but_normal.png", LoadMethod.Single);
            But_Hover = But_Norm;
            But_Press = But_Norm;
            SmallFont = new VFont("data/font/times.ttf.vf");
            BigFont = new VFont("data/font/f2.ttf.vf");
            WinBord = new VTex2D("data/ui/skin/neonblue/winbord.png", LoadMethod.Single);
            WinCon = new VTex2D("data/ui/skin/neonblue/wincon.png", LoadMethod.Single);
            WinTitle = new VTex2D("data/ui/skin/neonblue/wintitle.png", LoadMethod.Single);
            WinBackCol.W = 0.5f;
            Click = new Sound.VSoundSource("data/ui/skin/neonblue/click.wav");
            Type = new Sound.VSoundSource("data/ui/skin/neonblue/type.wav");
            EOE = new Sound.VSoundSource("data/ui/skin/neonblue/eoe.wav");
            WinBackCol = new Vector4(0.9f, 0.9f, 0.9f, 0.9f);
        }
        public override void DrawButton(UIButton b)
        {
            VTex2D bi;
            bi = StateImg(b.State);
            int fw = SmallFont.Width(b.Name);
            int fh = SmallFont.Height();
            Vector4 col = new Vector4(1, 1, 1, 1*UISys.AlphaMod);
            switch(b.State)
            {
                case ButState.Norm:
                    col = new Vector4(0.6f, 0.6f, 0.6f, 0.6f*UISys.AlphaMod);
                    break;
                case ButState.Hover:
                    col = new Vector4(0.8f, 0.8f, 0.8f, 0.8f*UISys.AlphaMod);
                    break;
                case ButState.Press:
                    col = Vector4.One;
                    col.W = col.W * UISys.AlphaMod;
                    break;
            }
            VPen.BlendMod = VBlend.Alpha;
            VPen.Rect((int)b.WidX, (int)b.WidY, (int)b.WidW, (int)b.WidH, bi, col);
            VFontRenderer.Draw(SmallFont, b.Name, (int)(b.WidX + b.WidW / 2 - (fw / 2)),(int)( b.WidY + (b.WidH) / 2 - (fh / 2)),new Vector4(1,1,1,UISys.AlphaMod));
            if(b==UISys.Active)
            {
             //   VPen.Line(b.WidX, b.WidY + 4, b.WidX + b.WidW, b.WidY + 4, new Vector4(1, 1, 1, 1));
            }
        }
        public override void DrawPanel(UIPanel p)
        {
            if (p.Flat)
            {
                VPen.Rect(p.WidX, p.WidY, p.WidW, p.WidH, new Vector4(0.7f, 0.7f, 0.7f, 0.8f));
            }
            else { 
                VPen.Rect(p.WidX, p.WidY, p.WidW, p.WidH, PanelBG, new Vector4(0.7f, 0.7f, 0.7f, 0.9f));
            }
                VFontRenderer.Draw(SmallFont, p.Name, p.WidX+5, p.WidY+5);
        }
        public override void DrawWindow(UIWindow w)
        {
            WinBackCol.W = w.Alpha * UISys.AlphaMod;
        //    VPen.Rect(w.WidX, w.WidY, w.WidW, w.WidH, WinBord, WinBackCol);
            VPen.Rect(w.WidX, w.WidY, w.WidW, w.WidH, WinCon, WinBackCol);
            if (w.DrawTitle)
            {
                WinTitCol.W = w.Alpha * UISys.AlphaMod;
                VPen.Rect(w.WidX, w.WidY, w.WidW, TitleHeight, WinTitle, WinTitCol);
                VFontRenderer.Draw(SmallFont, w.Name, w.WidX + 5, w.WidY + 2, new Vector4(1, 1, 1, UISys.AlphaMod));
            }
        }
        public override void DrawLine(int x, int y, int x2, int y2, Vector4 col)
        {
            VPen.Line(x, y, x2, y2, col);
        }
        public override void DrawBox(int x, int y, int w, int h)
        {
            VPen.Rect(x, y, w, h, new Vector4(0.2f, 0.2f, 0.2f, 0.8f*UISys.AlphaMod));
            VPen.Rect(x + 2, y + 2, w - 4, h - 4, new Vector4(0.9f, 0.9f, 0.9f, 0.8f*UISys.AlphaMod));
            
        }
        public override void DrawBoxText(int x, int y, string text)
        {

            VFontRenderer.Draw(SmallFont, text, x, y, new Vector4(0.1f, 0.1f, 0.1f, 0.9f * UISys.AlphaMod));
        }
    }
      
}
