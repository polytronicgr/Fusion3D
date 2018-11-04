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

        private Vector3 FixP ( Vector3 v )
        {
            float vX=v.X;
            v.X = -v.Y;
            v.Y = v.Z;
            v.Z = -vX;
            return v;
        }

        private Vector3 FixV ( Vector3 v )
        {
            float vY = v.Y;
            v.Y = -v.Z;
            v.Z = vY;
            return v;
        }

        private Vivid3D.Scene.GraphEntity3D ReadNodes ( )
        {
            Vivid3D.Scene.GraphEntity3D vn = new Vivid3D.Scene.GraphEntity3D();

            string node_name = ReadString ( );
            Console.WriteLine ( "Node:" + node_name );
            vn.Name = node_name;
            OpenTK.Matrix4 lmat = ReadMatrix4 ( );
            Matrix4 omat = ReadMatrix4();
            Matrix4 wmat = ReadMatrix4();
            Matrix4 nmat = ReadMatrix4();
            Quaternion qr = ReadQuat();
            //mat.Transpose ( );
            Matrix4 mat = wmat;

            vn.LocalPos = FixP ( mat.ExtractTranslation ( ) );
            //vn.LocalPos = new Vector3 ( vn.LocalPos.X, -vn.LocalPos.Y, vn.LocalPos.Z );
            //vn.LocalScale = mat.ExtractScale ( );
            mat = mat.ClearTranslation ( );
            mat = mat.ClearScale ( );
            //mat.Transpose ( );
            //Vector4 c0= mat.Column1;
            //mat.Column1 = mat.Column2;
            //mat.Column2 = c0;
            //float cx = mat.Column0.X;
            //Vector4 c0 = mat.Column0;
            //c0.X = c0.Y;
            //c0.Y = cx;
            // mat.Column0 = c0;
            //mat.Column0.X = mat.Column0.Y;
            Matrix4 lm = Matrix4.CreateFromQuaternion(qr);
            //vn.LocalTurn = Ma mat; //mat;// Matrix4.Identity;
            vn.LocalTurn = lmat;
            Console.WriteLine ( "Quat:" + qr );

            int vc = ReadInt();

            Console.WriteLine ( "verts:" + vc );
            List<OpenTK.Vector3> normL = new List<Vector3>();
            List<Vector3> tanL = new List<Vector3>();
            List<Vector3> biL = new List<Vector3>();
            List<OpenTK.Vector3> posL = new List<OpenTK.Vector3>();
            List<Vector3> uvL = new List<Vector3>();
            //Vivid3D.Data.VMesh msh = new Vivid3D.Data.VMesh ( ic, vc );
            for ( int v = 0; v < vc; v++ )
            {
                OpenTK.Vector3 v_pos = ReadVec3 ( );
                Vector3 n = ReadVec3();
                Vector3 t = ReadVec3();
                Vector3 b = ReadVec3();
                Vector3 uv = ReadVec3();
                //posL.Add ( v_pos );
                //float vy = v_pos.Y;
                //v_pos.Y = v_pos.Z;
                //v_pos.X = vy;
                //v_pos.Y = -v_pos.Y;
                // float vX = v_pos.X;
                //  v_pos.X = v_pos.Z;
                //   v_pos.Z = vX;
                //     float vX = v_pos.X;
                //   v_pos.X = v_pos.Z;
                //  v_pos.Z = -vX;
                //  vX = v_pos.X;
                // v_pos.X = v_pos.Y;
                // v_pos.Y = vX;

                posL.Add ( v_pos );
                normL.Add ( n );
                tanL.Add ( t );
                biL.Add ( b );
                uvL.Add ( uv );

                //Console.WriteLine ( "V:" + v_pos );
            }

            int ic = ReadInt();

            int vi=0;
            Vivid3D.Data.VMesh msh = new Vivid3D.Data.VMesh ( ic*3, vc );
            foreach ( Vector3 vp in posL )
            {
                Vector3 vt = tanL[vi];
                Vector3 vb = biL[vi];
                Vector3 vn2 = normL[vi];
                Vector2 vuv = new Vector2(uvL[vi].X,uvL[vi].Y);

                msh.SetVertex ( vi, vp, vt, vb, vn2, vuv );

                vi++;
            }

            for ( int f = 0; f < ic; f++ )
            {
                int v0,v1,v2;
                v0 = ReadInt ( ); v1 = ReadInt ( ); v2 = ReadInt ( );
                msh.SetTri ( f, v0, v1, v2 );
            }

            // Matrix4 tm = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-180)); msh.Transform
            // ( tm );

            //tm = tm * Matrix4.CreateRotationY ( MathHelper.DegreesToRadians ( 90 ) );
            // msh.Transform ( tm );

            //tm = tm * Matrix4.CreateRotationX ( MathHelper.DegreesToRadians ( 90 ) );
            // msh.Transform ( tm );

            Matrix4 tm = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90));
            tm = tm * Matrix4.CreateRotationX ( MathHelper.DegreesToRadians ( -90 ) );

            vn.LocalTurn = vn.LocalTurn * tm;

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