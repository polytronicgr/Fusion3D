using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
using PhysX.VisualDebugger;
namespace Vivid3D.Physics
{
    public class PhysicsManager
    {
         public class ECB : ErrorCallback
        {
            public override void ReportError(ErrorCode errorCode, string message, string file, int lineNumber)
            {
                Console.WriteLine("ErrCode:" + errorCode.ToString() + " Msg:" + message + " File:" + file + " line:" + lineNumber);
            }
        }
        public static List<PyObject> Objs = new List<PyObject>();

        public static PhysX.Physics py;
        public static SceneDesc SceneD;
        public static PhysX.Scene Scene;
        public static void AddObj(PyObject obj)
        {
            Objs.Add(obj);
        }

        public static void Update(float time)
        {

            Scene.Simulate(time);
            Scene.FetchResults(block: true);
            foreach(var obj in Objs)
            {
                if (obj is PyDynamic)
                {
                    obj.Grab();
                }
            }
        }
        public static PhysX.VisualDebugger.Pvd pvd;
        public static void InitSDK()
        {
            void er()
            {

            }
            Foundation fd = new Foundation(new ECB());
            pvd = new Pvd(fd);
            py = new PhysX.Physics(fd,true,pvd);
            SceneD = new SceneDesc();
     
            SceneD.Gravity = new System.Numerics.Vector3(0, -9, 0);
            Scene = py.CreateScene(SceneD);
            Scene.SetVisualizationParameter(VisualizationParameter.Scale, 2.0f);
            Scene.SetVisualizationParameter(VisualizationParameter.CollisionShapes, true);
            Scene.SetVisualizationParameter(VisualizationParameter.ActorAxes, true);
            py.Pvd.Connect("localhost");
           // Scene.Gravity = new System.Numerics.Vector3(0, 0, 0);
                      
            //PhysX.Material dm = Scene.get


        }

    }
}
