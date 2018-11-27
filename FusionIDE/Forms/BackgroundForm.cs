using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Resonance;
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
                sx = sy = sl = 0;
                LineType st = LineType.Horiz;
                int lt = r.Next(2);
                BGLine bl = null;
                switch (lt) {
                    case 0:
                        sx = 0;
                        sy = r.Next(FusionEngine.App.AppInfo.H);
                        st = LineType.Horiz;
                        sl = FusionEngine.App.AppInfo.W;

                    break;
                    case 1:
                        sy = 0;
                        sx = r.Next(FusionEngine.App.AppInfo.W);
                        st = LineType.Vert;
                        sl = FusionEngine.App.AppInfo.H;

                        break;
                }
                bl = new BGLine(sx, sy, sl, st);
                Lines.Add(bl);
            }


            Draw = () =>
            {
                FusionEngine.Draw.Pen2D.BlendMod = FusionEngine.Draw.PenBlend.Solid;
                Random R = new Random(Environment.TickCount);
                foreach(var line in Lines)
                {

                    switch (line.Type)
                    {
                    
                        case LineType.Horiz:

                            if (line.Sub == false)
                            {

                                int ds = r.Next(30);
                                if (ds == 10)
                                {

                                    line.Sub = true;
                                    line.SP = r.Next(line.L);

                                    line.SL = r.Next(line.L / 4);
                                    line.SD = r.Next(2);
                                    line.Sub = true;
                                    line.SD = r.Next(2);


                                }

                            }

                            //DrawLine(0, line.Y, line.L, line.Y, new OpenTK.Vector4(1, 1, 1, 1));
                            DrawFormSolid(new OpenTK.Vector4(0,0.4f,0.4f,0.4f), 0, line.Y, line.L, 2);

                            if (line.Sub)
                            {
                                switch (line.SD)
                                {
                                    case 0:
                                        line.SP = line.SP - 3;
                                        if(line.SP < 0){
                                            line.SD = 1;
                                        }
                                    
                                        break;
                                    case 1:
                                        line.SP = line.SP + 3;
                                        if (line.SP > line.L)
                                        {
                                            line.SD = 0;
                                        }
                                        break;
                                }
                                DrawFormSolid(new OpenTK.Vector4(3, 3, 3, 3), line.SP, line.Y-4, line.SL, 8);

                            }

                            break;
                        case LineType.Vert:
                            if (line.Sub == false)
                            {

                             

                                int ds = r.Next(30);
                                if (ds == 10)
                                {
                                    line.SD = r.Next(2);
                                    line.Sub = true;
                                    line.SP = r.Next(line.L);

                                    line.SL = r.Next(line.L / 4);
                                    line.SD = r.Next(2);
                                    line.Sub = true;



                                }

                            }
                            switch (line.SD)
                            {
                                case 0:
                                    line.SP = line.SP - 3;
                                    if (line.SP < 0)
                                    {
                                        line.SD = 1;
                                    }

                                    break;
                                case 1:
                                    line.SP = line.SP + 3;
                                    if (line.SP > line.L)
                                    {
                                        line.SD = 0;
                                    }
                                    break;
                            }


                            //DrawLine(line.X, 0, line.X, line.L, new OpenTK.Vector4(1, 1, 1, 1));
                            DrawFormSolid(new OpenTK.Vector4(0.4f,0.4f,0.4f,0.4f), line.X, 0, 2, line.L);
                            if (line.Sub)
                            {
                                DrawFormSolid(new OpenTK.Vector4(3, 3, 3, 3), line.X-4, line.SP, 8, line.SL);
                            }


                            break;
                    }

                }

            };


        }

    }
    public class BGLine
    {
        public LineType Type = LineType.Horiz;
        public int X, Y, L;
        public bool Sub = false;
        public int SP = 0;
        public int SL = 0;
        public int SD = 0;
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
