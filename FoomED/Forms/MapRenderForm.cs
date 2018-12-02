using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Resonance;
using FusionEngine.Resonance.Forms;

namespace FoomED.Forms
{
    public class MapRenderForm : WindowForm
    {

        public InvaderEng.Map.Map Map;
        public InvaderEng.Render.MapRenderer Render;
        public MapRenderArea Area;
        public MapRenderForm()
        {

            Area = new MapRenderArea();
            Area.RenF = this;
            body.Add(Area);
            SubChanged = () =>
            {
               
                Area.X = 5;
                Area.Y = 30;
                Area.W = body.W - 10;
                Area.H = body.H - 40;
            };

        }

    }
    public class MapRenderArea : UIForm
    {
        public MapRenderForm RenF = null;
        public MapRenderArea()
        {

            Draw = () =>
            {

                DrawFormSolid(new OpenTK.Vector4(1, 0, 0, 1));

                RenF.Render.RenX = GX;
                RenF.Render.RenY = GY;
                RenF.Render.RenW = W;
                RenF.Render.RenH = H;
                RenF.Render.Render();

            };

        }

    }
}
