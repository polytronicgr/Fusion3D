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
        public int UsedLines = 1;
        public float CodeX = 0;
        public float CodeY = 0;
        public float CodeDX = 0;
        public float CodeDY = 0;
        public TextAreaForm()
        {

            Lines[0] = "This is a test string wrote in actual C# sharp code - you may disagree, but you may not escape.";
            Lines[1] = "This is also a test line. You may disagree, but then i'll cut yer!(Mr Mann.2018)";
            Lines[2] = "Another test string this time in english.";
            UsedLines = 3;

            Draw = () =>
            {

                DrawFormSolid(new Vector4(0,0.2f,0.3f, 0.7f), X, Y, W, H);

                int dx = 0, dy = 0;

                int skipped = 0;
                int line_num = 0;
                int draw_y = 5;
                foreach(var line in Lines)
                {

                    line_num++;
                    if (line_num > UsedLines) break;
                    if (CodeY > 0)
                    {
                        if (CodeY < skipped)
                        {
                            skipped++;
                            continue;
                        }
                    }

                    var this_string = line.Substring((int)CodeX);

                    DrawText(this_string, 5,Y+draw_y,new Vector4(0.8f,0.8f,0.8f,1));
                    draw_y += 25;
                   

                }


            };

        }



    }
}
