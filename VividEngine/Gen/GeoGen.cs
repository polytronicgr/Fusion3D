using OpenTK;
using Vivid3D.Data;
using Vivid3D.Scene;

namespace Vivid3D.Gen
{
    public class GeoGen
    {
        public static GraphEntity3D Quad ( int w, int h )
        {
            GraphEntity3D r = new GraphEntity3D();

            VMesh mesh = new VMesh(6, 4);

            Vector3 v1 = new Vector3(-w, -h, 0);
            Vector3 v2 = new Vector3(w, -h, 0);
            Vector3 v3 = new Vector3(w, h, 0);
            Vector3 v4 = new Vector3(-w, h, 0);

            Vector3 z = new Vector3(0, 0, 0);

            mesh.SetVertex ( 0, v1, z, z, z, new Vector2 ( 0, 0 ) );
            mesh.SetVertex ( 1, v2, z, z, z, new Vector2 ( 1, 0 ) );
            mesh.SetVertex ( 2, v3, z, z, z, new Vector2 ( 1, 1 ) );
            mesh.SetVertex ( 3, v4, z, z, z, new Vector2 ( 0, 1 ) );

            mesh.SetTri ( 0, 0, 1, 2 );
            mesh.SetTri ( 1, 2, 3, 0 );

            /*
                        mesh.SetIndex(0, 0);
            mesh.SetIndex(1, 1);
            mesh.SetIndex(2, 2);
            mesh.SetIndex(3, 2);
            mesh.SetIndex(4, 3);
            mesh.SetIndex(5, 0);
            */
            mesh.Final ( );

            r.AddMesh ( mesh );

            return r;
        }

        public static void Cube ( int w, int h, int d )
        {
            GraphEntity3D ent = new GraphEntity3D();

            VMesh mesh = new VMesh(36, 8);

            Material.Material3D mat = new Material.Material3D ( );

            Vector3 p1 = new Vector3(-w / 2, -h / 2, -d / 2);
            Vector3 p2 = new Vector3(w / 2, -h / 2, -d / 2);
            Vector3 p3 = new Vector3(w / 2, h / 2, -d / 2);
            Vector3 p4 = new Vector3(-w / 2, h / 2, -d / 2);

            Vector3 p12 = new Vector3(-w / 2, -h / 2, d / 2);
            Vector3 p22 = new Vector3(w / 2, -h / 2, d / 2);
            Vector3 p32 = new Vector3(w / 2, h / 2, d / 2);
            Vector3 p42 = new Vector3(-w / 2, h / 2, d / 2);

            Vector3 z = new Vector3(0, 0, 0);

            // mesh.SetVertex(0,p1,)
        }
    }
}