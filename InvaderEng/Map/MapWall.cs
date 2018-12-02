using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvaderEng.Map
{
    public class MapWall : MapElement
    {

        public float X1, Z1;
        public float X2, Z2;
        public float Height;

        public MapWall(int x1, int z1, int x2, int z2, int height = 30)
        {
            X1 = x1;
            Z1 = z1;

            X2 = x2;
            Z2 = z2;
            Height = height;
        }

    }
}
