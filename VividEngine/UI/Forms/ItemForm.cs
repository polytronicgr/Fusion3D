using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;
using Vivid3D.App;
namespace Vivid3D.Resonance.Forms
{
    public class ItemForm : UIForm
    {
        public bool Act = false;
        public VTex2D Pic = null;
        public bool Render = true;
         public ItemForm()
        {

            void ActiveFunc()
            {
                Act = true;
            }

            void DeactiveFunc()
            {
                Act = false;
            }
            Activate = ActiveFunc;
            Deactivate = DeactiveFunc;
            void DrawFunc()
            {
                if (!Render) return;
                if (Act)
                {
                    DrawFormSolid(new Vector4(0.2f, 0.2f, 0.4f, 0.8f), 0, 0, W, H);
                }
                    if (Pic != null)
                {

                    DrawForm(Pic, 0, 0, 28, 20);
                    DrawText(Text, 38, 0);
                }
                else
                {
                    DrawText(Text, 0, 0);
                }
            }

            void MouseDownFunc(int b)
            {
                Click?.Invoke(b);
            }

            MouseDown = MouseDownFunc;

            void DoubleClickFunc(int b)
            {
                Console.WriteLine("DoubleClicked:" + Text);
            }

            Draw = DrawFunc;

        }

    }
}
