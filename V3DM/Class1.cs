using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace V3DM
{
    public class ImportV3D
    {
        public FileStream _F;
        public BinaryReader _R;

        public Vivid3D.Scene.GraphNode3D ImportMesh ( string file )
        {
            _F = new FileStream ( file, FileMode.Open, FileAccess.Read );

            if ( _F == null )
            {
                Console.WriteLine ( "Unable to open V3DM file." );
                return null;
            }
            _R = new BinaryReader ( _F );

            // CheckHeader ( );

            Vivid3D.Scene.GraphEntity3D root = new Vivid3D.Scene.GraphEntity3D
            {
                LocalTurn = Matrix4.Identity
            };

            int nc = ReadInt();

            Console.WriteLine ( "TopLevelNodes:" + nc );

            for ( int i = 0; i < nc; i++ )
            {
                root.Add ( ReadNodes ( ) );
            }

            return root;

            return null;
        }

        private Vivid3D.Scene.GraphEntity3D ReadNodes ( )
        {
            Vivid3D.Scene.GraphEntity3D vn = new Vivid3D.Scene.GraphEntity3D();

            string node_name = ReadString ( );
            Console.WriteLine ( "Node:" + node_name );
            vn.Name = node_name;
            OpenTK.Matrix4 mat = ReadMatrix4 ( );
            //mat.Transpose ( );

            vn.LocalPos = mat.ExtractTranslation ( );
            vn.LocalPos = new Vector3 ( vn.LocalPos.X, -vn.LocalPos.Y, vn.LocalPos.Z );
            vn.LocalScale = mat.ExtractScale ( );
            mat = mat.ClearTranslation ( );
            mat = mat.ClearScale ( );
            //mat.Transpose ( );
            vn.LocalTurn = Matrix4.Identity; //mat;// Matrix4.Identity;

            int vc = ReadInt();

            Console.WriteLine ( "verts:" + vc );

            List<OpenTK.Vector3> posL = new List<OpenTK.Vector3>();
            //Vivid3D.Data.VMesh msh = new Vivid3D.Data.VMesh ( ic, vc );
            for ( int v = 0; v < vc; v++ )
            {
                OpenTK.Vector3 v_pos = ReadVec3 ( );
                //posL.Add ( v_pos );
                float vy = v_pos.Y;
                v_pos.Y = -v_pos.Z;
                v_pos.Z = vy;
                posL.Add ( v_pos );

                //Console.WriteLine ( "V:" + v_pos );
            }

            int ic = ReadInt();

            int vi=0;
            Vivid3D.Data.VMesh msh = new Vivid3D.Data.VMesh ( ic*3, vc );
            foreach ( Vector3 vp in posL )
            {
                Vector3 vt = Vector3.Zero;
                Vector3 vb = Vector3.Zero;
                Vector3 vn2 = Vector3.Zero;
                Vector2 vuv = Vector2.Zero;

                msh.SetVertex ( vi, vp, vt, vb, vn2, vuv );

                vi++;
            }

            for ( int f = 0; f < ic; f++ )
            {
                int v0,v1,v2;
                v0 = ReadInt ( ); v1 = ReadInt ( ); v2 = ReadInt ( );
                msh.SetTri ( f, v0, v1, v2 );
            }
            msh.Mat = new Vivid3D.Material.Material3D ( );
            vn.AddMesh ( msh );

            msh.Final ( );

            int nc = ReadInt();
            //int nc=0;
            Console.WriteLine ( "childNodes:" + nc );

            for ( int i = 0; i < nc; i++ )
            {
                vn.Add ( ReadNodes ( ) );
            }
            // _R.ReadInt32 ( );
            return vn;
        }

        private bool CheckHeader ( )
        {
            byte[] head = _R.ReadBytes(4);
            string hs = Encoding.UTF8.GetString(head);

            Console.WriteLine ( hs );
            if ( hs != "V3DM" )
            {
                Console.WriteLine ( "V3DM header check faliure." );
                return false;
            }
            else
            {
                Console.WriteLine ( "V3DM header check pass." );
            }
            return true;
        }

        private int ReadInt ( )
        {
            return _R.ReadInt32 ( );
        }

        private float ReadFloat ( )
        {
            return _R.ReadSingle ( );
        }

        private OpenTK.Matrix4 ReadMatrix4 ( )
        {
            OpenTK.Matrix4 m = new OpenTK.Matrix4
            {
                Column0 = ReadVec4 ( ),
                Column1 = ReadVec4 ( ),
                Column2 = ReadVec4 ( ),
                Column3 = ReadVec4 ( )
            };

            return m;
        }

        private OpenTK.Vector3 ReadVec3 ( )
        {
            return new OpenTK.Vector3 ( ReadFloat ( ), ReadFloat ( ), ReadFloat ( ) );
        }

        private OpenTK.Vector4 ReadVec4 ( )
        {
            return new OpenTK.Vector4 ( ReadFloat ( ), ReadFloat ( ), ReadFloat ( ), ReadFloat ( ) );
        }

        private OpenTK.Quaternion ReadQuat ( )
        {
            return new OpenTK.Quaternion ( ReadFloat ( ), ReadFloat ( ), ReadFloat ( ), ReadFloat ( ) );
        }

        private string ReadString ( )
        {
            int sl = ReadInt();

            Console.WriteLine ( "SL:" + sl );
            byte[] str = _R.ReadBytes(sl);
            return Encoding.UTF8.GetString ( str );
        }
    }
}