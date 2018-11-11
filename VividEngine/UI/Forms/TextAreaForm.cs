using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
namespace Vivid3D.Resonance.Forms
{
    public class TextAreaForm : UIForm 
    {
        public string[] Lines = new string[4096];
        public Vector2[] LinePos = new Vector2[4096];
        public int UsedLines = 1;
        public float CodeX = 0;
        public float CodeY = 0;
        public float CodeDX = 0;
        public float CodeDY = 0;
        public int EditX=0, EditY=0;
        public float ScrollSpeed = 0.35f;
        public int BiggestX
        {
            get
            {
                int line_num = 0;
                int big_x = 0;
                foreach(var line in Lines)
                {
                    int len = line.Length;
                    if (len > big_x) big_x = len;
                    line_num++;
                    if (line_num == UsedLines) return big_x;
                }
                return 0;
            }
        }
        public TextAreaForm()
        {

            CodeY = 1;
            Lines[0] = "This is a test string wrote in actual C# sharp code - you may disagree, but you may not escape.";
            Lines[1] = "This is also a test line. You may disagree, but then i'll cut yer!(Mr Mann.2018)";
            Lines[2] = "Another test string this time in english.";
            UsedLines = 3;

            KeyPress = (k,shift) =>
            {

                switch (k)
                {
                    case OpenTK.Input.Key.Left:
                        if (EditX > 0) EditX--;// CodeX--;
                        break;
                    case OpenTK.Input.Key.Right:
                        EditX++;
                        if (EditY < UsedLines)
                        {
                            if (EditX >= Lines[EditY].Length)
                            {
                                CodeX = CodeX + 1;
                            }
                        }
                        break;
                    case OpenTK.Input.Key.Up:
                        if (EditY > 0) EditY--;
                        if (EditY < CodeY) CodeY--;
                        break;
                    case OpenTK.Input.Key.Down:
                        EditY++;
                        if (EditY >= UsedLines)
                        {
                            CodeY++;
                        }
                        break;
                    default:
                        string key_text = k.ToString();
                        string s1, s2;

                        string this_string = Lines[real_line];

                        if (EditX == 0)
                        {
                            Lines[real_line] = key_text + this_string;
                            EditX++;

                        }
                        else
                        {
                            s1 = this_string.Substring(0, EditX);
                            s2 = this_string.Substring(EditX);
                            Lines[real_line] = s1 + key_text + s2;
                            EditX++;
                        }

                        break;
                }


            };

            Draw = () =>
            {

                DrawFormSolid(new Vector4(0,0.2f,0.3f, 0.7f), 0, 0, W, H);

                int dx = 0, dy = 0;

                int skipped = 0;
                int line_num = 0;
                int draw_y = 5;

                CodeDX = CodeDX + (CodeX - CodeDX) * ScrollSpeed;
                CodeDY = CodeDY + (CodeY - CodeDY) * ScrollSpeed;

                int ex = EditX - (int)CodeX;

                int ey = EditY;

                foreach(var line in Lines)
                {

                    line_num++;
                    if (line_num > UsedLines) break;
                    if (CodeY > 0)
                    {
                        if (CodeY > skipped)
                        {
                            LinePos[line_num - 1] = new Vector2(0, 10);
                            skipped++;
                            continue;
                        }
                    }

                    if ((int)CodeX < line.Length){
                        var this_string = line;// line.Substring((int)CodeX);

                        Vector2 line_pos = LinePos[line_num - 1];

                        int realx = 0;
                        for(int i = 0; i < (int)CodeX; i++)
                        {
                            string ch = this_string.Substring(i, 1);
                            realx += UI.Font.Width(ch);
                        }

                        line_pos.X = line_pos.X + ((5)-line_pos.X)  *ScrollSpeed;
                        line_pos.Y = line_pos.Y + ((5+draw_y) - line_pos.Y) * ScrollSpeed;
                        LinePos[line_num - 1] = line_pos;
                        this_string = this_string.Substring((int)CodeX);



                        if ((line_num - 1) == ey)
                        {
                            DrawFormSolid(new Vector4(0.6f, 0.6f, 0.6f, 0.6f), 0, (int)line_pos.Y, W, 20);
                            real_line = line_num - 1;
                            string s1, s2;

                            if (ex < this_string.Length)
                            {
                                s1 = this_string.Substring(0, ex);
                                s2 = this_string.Substring(ex);
                            }
                            else
                            {
                                s1 = this_string;
                                s2 = "";
                            }
                            int rx = 0;
                            if(ex>=this_string.Length)
                            {
                                ex = this_string.Length - 1;
                                rx = (ex * 10);
                                

                            }
                            for(int r = 0; r < ex; r++)
                            {
                                rx += UI.Font.Width(this_string.Substring(r, 1));
                            }

                            int ciy = (line_num-1) * 25;
                            DrawText(s1+s2, (int)line_pos.X, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                            DrawFormSolid(new Vector4(0, 0, 0, 1), (int)line_pos.X + rx, (int)line_pos.Y, 2, 10) ;
                            //DrawText(s2, (int)line_pos.X + 15, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));

                        }
                        else
                        {
                            DrawText(this_string, (int)line_pos.X, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1));
                        }
                    }
                    //else
                   

                    
                        draw_y += 25;
                    

                }


            };

        }
        int real_line = 0;



    }
}
