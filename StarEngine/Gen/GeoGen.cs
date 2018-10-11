using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using Vivid3D.Data;
using OpenTK;
namespace Vivid3D.Gen
{
    public class GeoGen
    {
        public static void Cube(int w,int h,int d)
        {

            var ent = new GraphEntity3D();

            var mesh = new VMesh(36, 8);

            var p1 = new Vector3(-w / 2, -h / 2, -d / 2);
            var p2 = new Vector3(w / 2, -h / 2, -d / 2);
            var p3 = new Vector3(w / 2, h / 2, -d / 2);
            var p4 = new Vector3(-w / 2, h / 2, -d / 2);

            var p12 = new Vector3(-w / 2, -h / 2, d / 2);
            var p22 = new Vector3(w / 2, -h / 2, d / 2);
            var p32 = new Vector3(w / 2, h / 2, d / 2);
            var p42 = new Vector3(-w / 2, h / 2, d / 2);

            var z = new Vector3(0, 0, 0);

//            mesh.SetVertex(0,p1,)

        }
    }
}
