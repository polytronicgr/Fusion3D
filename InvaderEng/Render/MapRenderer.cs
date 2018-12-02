using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvaderEng.Map;
using FusionEngine.Draw;
namespace InvaderEng.Render
{
    public class MapRenderer
    {

        public Map.Map Map = null;

        public int RenX, RenY;
        public int RenW, RenH;

        public MapRenderer()
        {

        }

        public void Render()
        {
            float a = 0;

            float ai = 1.0f / (float)RenW;

            for(int x = 0; x < RenW; x++)
            {

                Pen2D.Rect(RenX+x, RenY, 2, RenH, new OpenTK.Vector4(a, a, a, 1.0f));

                a += ai;
            }

        }

    }
}
