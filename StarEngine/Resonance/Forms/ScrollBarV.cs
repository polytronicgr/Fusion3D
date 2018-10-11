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
    public class ScrollBarV : UIForm
    {

        public float Cur = 0;
        public float Max = 1;
        public float CurView = 0.1f;
        public VTex2D But;
        public ButtonForm ScrollBut;
        public ScrollBarV()
        {

            But = new VTex2D("Data\\UI\\Skin\\but_normal.png", LoadMethod.Single, true);
            ScrollBut = new ButtonForm().Set(0, 0, W, 10) as ButtonForm;

            void DrawFunc()
            {

                float DY = (GY + Y);

                float AY = Cur / Max;

                DrawFormSolid(new Vector4(0.3f, 0.3f, 0.3f, 0.8f));

            }


            void ChangedFunc()
            {
                ScrollBut.X = 0;
                //ScrollBut.Y = 0;

                ScrollBut.W = W;
                ScrollBut.H = 20;
            }

            Changed = ChangedFunc;

            Draw = DrawFunc;
            Add(ScrollBut);

            void DragFunc(int x,int y)
            {
                
                ScrollBut.Y += y;
                Console.WriteLine("Y:" + y + " SY:" + ScrollBut.Y);
       
                if (ScrollBut.Y < 0) ScrollBut.Y = 0;
                if (ScrollBut.Y > H - ScrollBut.H) ScrollBut.Y = H - ScrollBut.H;
                Cur = (ScrollBut.Y / Max);

            }

            ScrollBut.Drag = DragFunc;


        }

    }
}
