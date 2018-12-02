using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvaderEng.Map
{
    public class Map
    {
        public List<MapElement> Elements = new List<MapElement>();
        public MapCam Cam = new MapCam();

        public void WallBox(int cx,int cz,int size,int height = 30)
        {
            size = size / 2;
            var lW = new MapWall(-size, -size, -size, size, height);
            var tW = new MapWall(-size, size, size, size,height);
            var bW = new MapWall(-size, -size, size, -size, height);
            var rW = new MapWall(size, -size, size, size, height);

            Elements.Add(lW);
            Elements.Add(tW);
            Elements.Add(rW);
            Elements.Add(bW);


        }

    }
}
