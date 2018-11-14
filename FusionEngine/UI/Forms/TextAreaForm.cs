using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
namespace Fusion3D.Resonance.Forms
{
    public class TextAreaForm : UIForm 
    {
        public class KeyColor
        {
            public string Key = "";
            public Vector4 Col = Vector4.One;
            public KeyColor(string key, Vector4 col)
            {
                Key = key;
                Col = col;
            }
        }
        public string[] Lines = new string[4096];
        public Vector2[] LinePos = new Vector2[4096];
        public int UsedLines = 1;
        public float CodeX = 0;
        public float CodeY = 0;
        public float CodeDX = 0;
        public float CodeDY = 0;
        public int EditX=0, EditY=0;
        public float ScrollSpeed = 0.35f;
        public List<KeyColor> Keys = new List<KeyColor>();
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

            for(int i = 0; i < 4000; i++)
            {
                Lines[i] = string.Empty;
            }
            CodeY = 0;
            UsedLines = 1;
          //  Lines[0] = "This is a test string wrote in actual C# sharp code - you may disagree, but you may not escape.";
          //  Lines[1] = "This is also a test line. You may disagree, but then i'll cut yer!(Mr Mann.2018)";
           // Lines[2] = "Another test string this time in english.";
            

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
                    case OpenTK.Input.Key.Space:
                        string key_text4 = " ";//k.ToString();
                        if (!shift)
                        {
                            key_text4 = key_text4.ToLower();
                        }
                        string s14, s24;

                        string this_string4 = Lines[real_line];

                        if (EditX == 0)
                        {
                            Lines[real_line] = key_text4 + this_string4;
                            EditX = 1;

                        }
                        else
                        {
                            s14 = this_string4.Substring(0, EditX);
                            s24 = this_string4.Substring(EditX);
                            Lines[real_line] = s14 + key_text4 + s24;
                            EditX++;
                        }

                        break;
                    case OpenTK.Input.Key.Delete:

                        string key_text3 = k.ToString();
                        if (!shift)
                        {
                            key_text3 = key_text3.ToLower();
                        }
                        string s13, s23;

                        string this_string3 = Lines[real_line];

                        if (EditX == 0)
                        {


                        }
                        else
                        {
                            if (this_string3.Length > 1)
                            {
                                if (EditX < this_string3.Length-1)
                                {
                                    s13 = this_string3.Substring(0, EditX);
                                    s23 = this_string3.Substring(EditX + 1);
                                    Lines[real_line] = s13 + s23;
                                 //   EditX--;
                                }
                            }
                        }


                        break;
                    case OpenTK.Input.Key.BackSpace:

                        string key_text2 = k.ToString();
                        if (!shift)
                        {
                            key_text2 = key_text2.ToLower();
                        }
                        string s12, s22;

                        string this_string2 = Lines[real_line];

                        if (EditX == 0)
                        {

                            if (real_line > 0)
                            {
                                EditX = Lines[real_line - 1].Length;
                                if (EditY > 0)
                                {
                                    EditY--;
                                }
                            }
                        }
                        else
                        {
                            if (this_string2.Length > 1)
                            {
                                s12 = this_string2.Substring(0, EditX - 1);
                                s22 = this_string2.Substring(EditX);
                                Lines[real_line] = s12 + s22;
                                EditX--;
                            }
                            else
                            {
                                if (this_string2.Length > 0)
                                {
                                    Lines[real_line] = "";
                                    EditX--;
                                }
                            }
                        }

                        break;
                    case OpenTK.Input.Key.Enter:
                        var s15 = Lines[real_line].Substring(0, EditX);
                        var s25 = Lines[real_line].Substring(EditX);
                     
                        Lines[real_line] = s15;
                        for(int my = 4000;my>real_line; my--)
                        {
                            Lines[my] = Lines[my-1];
                            LinePos[my] = LinePos[my - 1];
                        }
                        Lines[real_line + 1] = s25;
                        UsedLines += 2;
                        EditY++;
                       

                        break;
                    default:
                        string key_text = k.ToString();

                        switch (k)
                        {
                            case OpenTK.Input.Key.Plus:
                                key_text = "=";
                               
                                if (shift) key_text = "+";
                                break;
                            case OpenTK.Input.Key.Minus:
                                key_text = "-";
                                if (shift) key_text = "_";
                                break;
                            case OpenTK.Input.Key.Period:
                                key_text = ".";
                                if (shift) key_text = "<";
                                break;
                            case OpenTK.Input.Key.Comma:
                                key_text = ",";
                                if (shift) key_text = ">";
                                break;
                            case OpenTK.Input.Key.Number0:
                                key_text = "0";
                                if (shift) key_text = ")";

                                break;
                            case OpenTK.Input.Key.Number1:
                                key_text = "1";
                                if (shift) key_text = "!";
                                break;
                            case OpenTK.Input.Key.Number2:
                                key_text = "2";
                                if (shift) key_text = "\"";
                                 break;
                            case OpenTK.Input.Key.Number3:
                                key_text = "3";
                                if (shift) key_text = "£";
                                break;
                            case OpenTK.Input.Key.Number4:
                                key_text = "4";
                                if (shift) key_text = "$";
                                break;
                            case OpenTK.Input.Key.Number5:
                                key_text = "5";
                                if (shift) key_text = "%";
                                break;
                            case OpenTK.Input.Key.Number6:
                                key_text = "6";
                                if (shift) key_text = "^";
                                break;
                            case OpenTK.Input.Key.Number7:
                                key_text = "7";
                                if (shift) key_text = "&";
                                break;
                            case OpenTK.Input.Key.Number8:
                                key_text = "8";
                                if (shift) key_text = "*";
                                break;
                            case OpenTK.Input.Key.Number9:
                                key_text = "9";
                                if (shift) key_text = "(";
                                break;
                            case OpenTK.Input.Key.Semicolon:
                                key_text = ";";
                                if (shift) key_text = ":";
                                break;
                            case OpenTK.Input.Key.Tab:
                                key_text = "    ";

                                break;
                            default:
                                if (!shift)
                                {
                                    key_text = key_text.ToLower();
                                }
                                break;
                        }

                      
                        string s1, s2;

                        string this_string = Lines[real_line];

                        if (EditX == 0)
                        {
                            Lines[real_line] = key_text + this_string;
                            EditX = 1;

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
                int time = Environment.TickCount;
                if (time > next_curs)
                {
                    next_curs = time + 400;
                    curs_on = curs_on ? false : true;
                }
                if (EditX > 0)
                {
                    if (EditX > Lines[EditY].Length)
                    {
                        EditX = Lines[EditY].Length;
                    }
                }

                CodeX = 0;
                CodeY = 0;

                DrawFormSolid(new Vector4(0,0.2f,0.3f, 0.7f), 0, 0, W, H);

                int dx = 0, dy = 0;

                int skipped = 0;
                int line_num = 0;
                int draw_y = 5;

                CodeDX = CodeDX + (CodeX - CodeDX) * ScrollSpeed;
                CodeDY = CodeDY + (CodeY - CodeDY) * ScrollSpeed;

                int ex = EditX - (int)CodeX;
             
                int ey = EditY;
                if (ex < 0) ex = 0;
                if (ey < 0) ey = 0;
                real_line = ey;
                foreach(var line in Lines)
                {

                    line_num++;
                    if (draw_y > H) break;
                    if (CodeY > 0)
                    {
                        if (CodeY > skipped)
                        {
                            LinePos[line_num - 1] = new Vector2(0, 10);
                            skipped++;
                            draw_y += 25;
                            continue;
                        }
                    }

                    if ((int)CodeX < line.Length+1){
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
                          
                            for(int r = 0; r < ex; r++)
                            {
                                string tb = (s1 + s2).Substring(r, 1);
                                rx += UI.Font.Width((s1+s2).Substring(r, 1));
                            }
                      
                            int ciy = (line_num-1) * 25;
                            DrawText2(s1+s2, (int)line_pos.X, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                            if(curs_on) DrawFormSolid(new Vector4(0, 0, 0, 1), (int)line_pos.X + rx, (int)line_pos.Y+2, 3, 15) ;
                            //DrawText(s2, (int)line_pos.X + 15, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1.0f));

                        }
                        else
                        {
                            DrawText2(this_string, (int)line_pos.X, (int)line_pos.Y, new Vector4(0.8f, 0.8f, 0.8f, 1));
                        }
                    }
                    //else

                    draw_y += 25;




                }
              

            };

        }
        public void DrawText2(string txt,int x,int y,Vector4 col)
        {
            if (txt == "") return;
            int px = 0;
            string print = "";
            for(int c = 0; c < txt.Length; c++)
            {

                print = print + txt[c];
                foreach(var key in Keys)
                {
                    if (print == key.Key)
                    {
                        DrawText(print, x + px, y, key.Col);
                        px = px + UI.Font.Width(print);
                        print = "";
                    }
                    if (print.Contains(key.Key))
                    {
                        int mi = print.IndexOf(key.Key);
                        string fp = print.Substring(0, mi);
                        string f2 = print.Substring(mi);
                        DrawText(fp, x + px, y, col);
                        px = px + UI.Font.Width(fp);
                        DrawText(key.Key, x + px, y, key.Col);
                        px = px + UI.Font.Width(key.Key);
                        print = "";

                    }
                }

                


            }
            DrawText(print, x + px, y, col);

        }
        int real_line = 0;
        bool curs_on = true;
        int next_curs = 0;


    }
}
