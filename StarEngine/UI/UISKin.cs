using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.App;
using Vivid3D.Draw;
using Vivid3D.UI.UIWidgets;
using Vivid3D.Font;
using OpenTK;
using Vivid3D.Sound;
namespace Vivid3D.UI
{
    public class UISkin
    {
        public VTex2D PanelBG;
        public VTex2D WinBord;
        public VTex2D WinCon;
        public VTex2D WinTitle;
        public VTex2D But_Norm;
        public VTex2D But_Hover;
        public VTex2D But_Press;
        public VFont BigFont = null;
        public VFont SmallFont = null;
        public Vector4 WinBackCol = new Vector4(0.9f, 0.9f, 0.9f, 0.96f);
        public Vector4 WinTitCol = new Vector4(0.9f, 0.9f, 0.9f, 0.96f);
        public Vector4 WinTitTextCol = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
        public Vector4 WinStatTextCol = new Vector4(0.2f, 0.7f, 0.2f, 1.0f);
        public float AlphaMod = 0.0f;
        public float DesAlphaMod = 1.0f;
        public VSoundSource Click;
        public VSoundSource EOE;
        public VSoundSource Type;
        public virtual int TitleHeight
        {
            get
            {
                return 20;
            }
        }
        public UISkin()
        {
            InitSkin();
        }
        public virtual void InitSkin()
        {

        }
        public virtual void DrawButton(UIButton w)
        {

        }
        public virtual void TypeSound()
        {
            Type.Play2D(false);
        }
        public virtual void EOESound()
        {
            EOE.Play2D(false);
        }
        public virtual void ClickSound()
        {
            Click.Play2D(false);
        }
        public virtual void DrawWindow(UIWindow w)
        {

        }
        public VTex2D StateImg(ButState bs)
        {
            VTex2D bi = null;
            switch (bs)
            {
                case ButState.Norm:
                    bi = But_Norm;
                    break;
                case ButState.Hover:
                    bi = But_Hover;
                    break;
                case ButState.Press:
                    bi = But_Press;
                    break;
            }

            return bi;
        }
        public virtual void DrawLine(int x,int y,int x2,int y2,Vector4 col)
        {

        }
        public virtual void DrawText(int x,int y,string text)
        {
            VFontRenderer.Draw(SmallFont, text, x, y);
        }
        public virtual void DrawText(int x,int y,string text,Vector4 col)
        {
            VFontRenderer.Draw(SmallFont, text, x, y, col);
        }
        public virtual void DrawPanel(UIPanel p)
        {

        }
        public virtual void DrawBoxText(int x,int y,string text)
        {

        }
        public virtual void DrawBox(int x,int y,int w,int h)
        {

        }
        public virtual void DrawRect(int x,int y,int w,int h)
        {
            DrawRect(x, y, w, h, Vector4.One);
        }
        public virtual void DrawRect(int x,int y,int w,int h,Vector4 c1)
        {
            DrawRect(x, y, w, h, c1, c1);
        }
        public virtual void DrawRect(int x,int y,int w,int h,Vector4 c1,Vector4 c2)
        {
            VPen.Rect(x, y, w, h, c1, c2);
        }
        public virtual void DrawImg(int x,int y,int w,int h,VTex2D img)
        {
            DrawImg(x, y, w, h, img, Vector4.One);
        }
        public virtual void DrawImg(int x,int y,int w,int h,VTex2D img,Vector4 col)
        {
            VPen.Rect(x, y, w, h, img, col);
        }
    }
    public enum ButState
    {
        Norm,Hover,Press
    }
}
