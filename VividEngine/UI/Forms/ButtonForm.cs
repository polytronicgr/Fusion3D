using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;
namespace Vivid3D.Resonance.Forms
{
    public class ButtonForm : UIForm
    {
        private bool Pressed = false, Over = false;
        private Vector4 NormCol = new Vector4(0.9f, 0.9f, 0.9f, 0.9f);
        private Vector4 OverCol = new Vector4(0.8f, 0.8f, 0.8f, 0.9f);
        private Vector4 PressCol = new Vector4(1, 1, 1, 1);
        public ButtonForm()
        {

            SetImage(new VTex2D("Data\\UI\\Skin\\but_normal.png", LoadMethod.Single, true));
            Col = NormCol;
            void DrawFunc()
            {
                VPen.BlendMod = VBlend.Alpha;
                
                DrawForm(CoreTex);
                DrawText(Text, W / 2 - UI.Font.Width(Text) / 2, H / 2 - UI.Font.Height() / 2);
            }

            void MouseEnterFunc()
            {

                if (Pressed == false)
                {
                    Col = OverCol;
                }
                Over = true;

            }

            void MouseLeaveFunc()
            {
                if (Pressed == false)
                {
                    Col = NormCol;
                }
                Over = false;
            }

            void MouseMoveFunc(int x,int y,int dx,int dy)
            {
                if (Pressed)
                {
                   // Drag?.Invoke(dx, dy);
                }
            }

            void MouseDownFunc(int b)
            {
                Col = PressCol;
                Pressed = true;
            }

            void MouseUpFunc(int b)
            {
                if (Over)
                {
                    Col = OverCol;
                }
                else
                {
                    Col = NormCol;
                }
                Pressed = false;
                Console.WriteLine("CLicked!");
                if (Click != null)
                {
                    Console.WriteLine("Has click");
                }
                Click?.Invoke(b);
            }

            Draw = DrawFunc;
            MouseEnter = MouseEnterFunc;
            MouseLeave = MouseLeaveFunc;
            MouseMove = MouseMoveFunc;
            MouseDown = MouseDownFunc;
            MouseUp = MouseUpFunc;
        }

    }
}
