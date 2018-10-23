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

        public static GraphEntity3D Quad(int w,int h)
        {
            var r = new GraphEntity3D();

            var mesh = new VMesh(6, 4);

            var v1 = new Vector3(-w, -h,0);
            var v2 = new Vector3(w, -h, 0);
            var v3 = new Vector3(w, h, 0);
            var v4 = new Vector3(-w, h, 0);

            var z = new Vector3(0, 0, 0);

            mesh.SetVertex(0, v1, z,z,z, new Vector2(0, 0));
            mesh.SetVertex(1, v2, z, z, z, new Vector2(1, 0));
            mesh.SetVertex(2, v3, z, z, z, new Vector2(1, 1));
            mesh.SetVertex(3, v4, z, z, z, new Vector2(0, 1));

            mesh.SetTri(0, 0, 1, 2);
            mesh.SetTri(1, 2, 3, 0);

            /*
                        mesh.SetIndex(0, 0);
            mesh.SetIndex(1, 1);
            mesh.SetIndex(2, 2);
            mesh.SetIndex(3, 2);
            mesh.SetIndex(4, 3);
            mesh.SetIndex(5, 0);
            */
            mesh.Final();

            r.AddMesh(mesh);

            return r;
        }

        public static void Cube(int w,int h,int d)
        {

            var ent = new GraphEntity3D();

            var mesh = new VMesh(36, 8);

            var mat = new Material.Material3D();



            var p1 = new Vector3(-w / 2, -h / 2, -d / 2);
            var p2 = new Vector3(w / 2, -h / 2, -d / 2);
            var p3 = new Vector3(w / 2, h / 2, -d / 2);
            var p4 = new Vector3(-w / 2, h / 2, -d / 2);

            var p12 = new Vector3(-w / 2, -h / 2, d / 2);
            var p22 = new Vector3(w / 2, -h / 2, d / 2);
            var p32 = new Vector3(w / 2, h / 2, d / 2);
            var p42 = new Vector3(-w / 2, h / 2, d / 2);

            var z = new Vector3(0, 0, 0);

//           mesh.SetVertex(0,p1,)

        }
    }
}
