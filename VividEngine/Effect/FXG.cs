using OpenTK;
using Vivid3D.Data;
using Vivid3D.Scene;

namespace Vivid3D.Effect
{
    public static class FXG
    {
        public static Effect3D FXOverride = null;
        public static Matrix4 Local = Matrix4.Identity;
        public static Matrix4 PrevLocal = Matrix4.Identity;
        public static Matrix4 Proj = Matrix4.Identity;
        public static GraphCam3D Cam = null;
        public static GraphEntity3D Ent = null;
        public static VMesh Mesh = null;
    }
}