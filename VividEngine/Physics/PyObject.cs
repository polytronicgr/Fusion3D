using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
using PhysX.VisualDebugger;
using System.IO;
namespace Vivid3D.Physics
{
    public class PyObject
    {
        
        public PhysX.Material Mat = null;
        public PyObject()
        {
            SetMat();
        }
        public void SetMat()
        {
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
        }
        public virtual void Create(Scene.GraphEntity3D ent)
        {

        }
        public virtual void Grab()
        {

        }
    }
    public class PyStatic : PyObject
    {
        public RigidStatic RID = null;
        public PhysX.Material Mat = null;
        public PhysX.Shape Shape;
        public PyStatic(Scene.GraphEntity3D ent)
        {
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
            CreateMesh(ent);
            PhysicsManager.AddObj(this);
        }
        public void CreateMesh(Scene.GraphEntity3D ent)
        {

            var verts = ent.GetAllVerts;
            System.Numerics.Vector3[] rvert = new System.Numerics.Vector3[verts.Count];

            int vi = 0;
            foreach (var v in verts)
            {


                rvert[vi] = new System.Numerics.Vector3(v.X, v.Y, v.Z);

                vi++;
            }


            int[] tris = new int[verts.Count];



            for (int i = 0; i < tris.Length; i++)
            {
                tris[i] = i;

            }




            var tm = new TriangleMeshDesc()
            {
                Flags = (MeshFlag)0,
                Triangles = tris,
                Points = rvert
            };

            var cook = PhysicsManager.py.CreateCooking();

            var str = new MemoryStream();
            var cookr = cook.CookTriangleMesh(tm, str);

            str.Position = 0;

            var trim = PhysicsManager.py.CreateTriangleMesh(str);

            var trig = new TriangleMeshGeometry(trim);


            RID = PhysicsManager.py.CreateRigidStatic();

            RID.CreateShape(trig, Mat);


            float m11 = ent.World.M11;
            float m12 = ent.World.M12;
            float m13 = ent.World.M13;
            float m14 = ent.World.M14;

            float m21 = ent.World.M21;
            float m22 = ent.World.M22;
            float m23 = ent.World.M23;
            float m24 = ent.World.M24;

            float m31 = ent.World.M31;
            float m32 = ent.World.M32;
            float m33 = ent.World.M33;
            float m34 = ent.World.M34;

            float m41 = ent.World.M41;
            float m42 = ent.World.M42;
            float m43 = ent.World.M43;
            float m44 = ent.World.M44;

                


            System.Numerics.Matrix4x4 tp = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

           // RID.GlobalPose = System.Numerics.Matrix4x4.CreateRotationX(-(float)System.Math.PI / 2);
            //RID.GlobalPosePosition = ent.LocalPos;


            PhysicsManager.Scene.AddActor(RID);



        }

    }
    public class PyDynamic : PyObject
    {
        public Scene.GraphEntity3D Sent;
        public RigidDynamic ID = null;
        public RigidStatic RID = null;
        public PhysX.Material Mat = null;
        public PhysX.Shape Shape;
        //    public System.Numerics.Matrix4x4 Pose;
        public override void Grab()
        {
            
            var mat = PoseTurn;
            var pos = PosePos;
       //     Console.WriteLine("Pos:" + pos.X + " " + pos.Y + " " + pos.Z);
            //  Sent.LocalPos = mat.ExtractTranslation();
            mat = mat.ClearTranslation();
            mat = mat.ClearScale();
            //var y = pos.Y;
            //pos.Y =-pos.Z;
            //pos.Z = -y;


            Sent.LocalPos = pos;
            Sent.LocalTurn = PoseTurn;
//            Sent.LocalTurn = mat;
        }
        public PyDynamic(PyType type,Scene.GraphEntity3D ent)
        {
            Sent = ent;
            PhysicsManager.AddObj(this);
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
            switch (type)
            {
                case PyType.Box:
                    CreateBox(ent);
                    break;
                
                    break;
            }
        }

        public OpenTK.Vector3 PosePos
        {
            get
            {
                var pp = ID.GlobalPosePosition;
                return new OpenTK.Vector3(pp.X, pp.Y, pp.Z);
            }
        }
        public OpenTK.Matrix4 PoseTurn
        {

            get
            {
                System.Numerics.Matrix4x4 m;

                m = ID.GlobalPose;

                m.Translation = new System.Numerics.Vector3(0, 0, 0);
                


                OpenTK.Matrix4 res;
                res = new OpenTK.Matrix4(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
                return res;
            }
            set
            {
                OpenTK.Matrix4 World = value;

                float m11 = World.M11;
                float m12 = World.M12;
                float m13 = World.M13;
                float m14 = World.M14;

                float m21 = World.M21;
                float m22 = World.M22;
                float m23 = World.M23;
                float m24 = World.M24;

                float m31 = World.M31;
                float m32 = World.M32;
                float m33 = World.M33;
                float m34 = World.M34;

                float m41 = World.M41;
                float m42 = World.M42;
                float m43 = World.M43;
                float m44 = World.M44;




                System.Numerics.Matrix4x4 tm = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

                ID.GlobalPose = tm;

            }
        }
      
        public void CreateBox(Scene.GraphEntity3D ent)
        {
            var bb = ent.Bounds;
            ID = PhysicsManager.py.CreateRigidDynamic();
            
            var ge = new BoxGeometry(bb.W / 2, bb.H / 2, bb.D / 2);
            Shape = ID.CreateShape(ge, Mat);
            ID.LinearVelocity = new System.Numerics.Vector3(0, 0, 0);


            //Pose = ID.GlobalPose;

            float m11 = ent.World.M11;
            float m12 = ent.World.M12;
            float m13 = ent.World.M13;
            float m14 = ent.World.M14;

            float m21 = ent.World.M21;
            float m22 = ent.World.M22;
            float m23 = ent.World.M23;
            float m24 = ent.World.M24;

            float m31 = ent.World.M31;
            float m32 = ent.World.M32;
            float m33 = ent.World.M33;
            float m34 = ent.World.M34;

            float m41 = ent.World.M41;
            float m42 = ent.World.M42;
            float m43 = ent.World.M43;
            float m44 = ent.World.M44;




            System.Numerics.Matrix4x4 tm = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

            ID.GlobalPose = tm;

            ID.SetMassAndUpdateInertia(3);



            Physics.PhysicsManager.Scene.AddActor(ID);






        }
        public void GetPose()
        {

        }
    }
}
