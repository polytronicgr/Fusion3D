using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Resonance;
namespace FusionIDE.Forms
{
    public class BackgroundForm : UIForm
    {
        public List<BGLine> Lines = new List<BGLine>();
        public BackgroundForm(int num_lines)
        {
            Random r = new Random(Environment.TickCount);
            for (int i = 0; i < num_lines; i++)
            {
                int sx, sy, sl;
                LineType st = LineType.Horiz;
                int lt = r.Next(1);
                BGLine bl = null;
                switch (lt) {
                    case 0:
                        sx = 0;
                        sy = r.Next(Vivid3D.App.AppInfo.H);
                        st = LineType.Horiz;
                        sl = Vivid3D.App.AppInfo.W;

                    break;
                    case 1:
                        sy = 0;
                        sx = r.Next(Vivid3D.App.AppInfo.W);
                        st = LineType.Vert;
                        sl = Vivid3D.App.AppInfo.H;

                        break;
                }
                bl = new BGLine(sx, sy, sl, st);
                Lines.Add(bl);
            }


            Draw = () =>
            {

                foreach(var line in Lines)
                {

                    

                }

            };


        }

    }
    public class BGLine
    {
        public LineType Type = LineType.Horiz;
        public int X, Y, L;
        public BGLine(int x,int y,int l,LineType t)
        {
            X = x;
            Y = y;
            L = l;
            Type = t;
          
        }
    }
    public enum LineType
    {
        Horiz,Vert
    }
}
