using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Vivid3D.Scene;
namespace Vivid3D.Pick
{
    public class PickResult
    {
        public Ray Ray;
        public Vector3 Pos;
        public Vector3 Norm;
        public Vector3 UV;
        public float Dist = 0.0f;
        public GraphNode3D Node;
        public int TriID = 0;
    }
}
