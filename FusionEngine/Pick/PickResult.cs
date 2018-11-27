using OpenTK;
using FusionEngine.Scene;

namespace FusionEngine.Pick
{
    public class PickResult
    {
        public Ray Ray;
        public Vector3 Pos;
        public Vector3 Norm;
        public Vector3 UV;
        public float Dist = 0.0f;
        public Node3D Node;
        public int TriID = 0;
    }
}