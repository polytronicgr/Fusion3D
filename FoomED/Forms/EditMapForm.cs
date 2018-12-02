using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Resonance;
using FusionEngine.FrameBuffer;
using FusionEngine.Texture;
using FusionEngine.Draw;
using FusionEngine.Resonance.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace FoomED.Forms
{
    public class EditMapForm : WindowForm
    {
        public ImageForm IMG;
        public FrameBufferColor FB = null;
        public InvaderEng.Map.Map EditMap = null;
        public float EditZ = 1.0f;
        public MapForm Edit = null;
        public EditMapForm()
        {
            EditMap = new InvaderEng.Map.Map();
            EditMap.WallBox(0, 0, 100, 30);
            Edit = new MapForm();
            Edit.Map = EditMap;


            //this.body.Add(Edit);

            //body.Add(IMG);

            //RenderMap();

            SubChanged = () =>
            {

              //  FB = new FrameBufferColor(W - 10, H - 80);
                Edit.Set(5, 30, body.W - 10, body.H - 40);
                //IMG.SetImage(FB.BB);

              
                if (!body.Forms.Contains(Edit))
                {
                    body.Add(Edit);
                }
                Edit.MouseDown = (b) =>
                {
                    Root.Forms.Remove(this);
                    Root.Forms.Add(this);
                };
                //RenderMap();
            };

        }

        public void RenderMap()
        {
      

        }

    }
}
