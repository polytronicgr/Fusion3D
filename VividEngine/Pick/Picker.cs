using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Vivid3D.Pick
{
    public struct Ray
    {
        public Vector3 pos;
        public Vector3 dir;
        public Ray(Vector3 p1,Vector3 p2)
        {
            pos = p1;
            dir = (p2 - p1).Normalized();
        }
    }
    public class Picker
    {

        private static Vector2 Convert(
  Vector3 pos,
  Matrix4 viewMatrix,
  Matrix4 projectionMatrix,
  int screenWidth,
  int screenHeight)
        {
           
            pos = Vector3.TransformPerspective(pos, viewMatrix);
            pos = Vector3.TransformPerspective(pos, projectionMatrix);
            pos.X /= pos.Z;
            pos.Y /= pos.Z;
            pos.X = (pos.X + 1) * screenWidth / 2;
            pos.Y = (pos.Y + 1) * screenHeight / 2;

            return new Vector2(pos.X,screenHeight- pos.Y);
        }

        public static Vector2 CamTo2D(Vivid3D.Scene.GraphCam3D cam,Vector3 pos)
        {

            return Convert(pos, cam.CamWorld, cam.ProjMat, Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H);

        }

        private const double Epsilon = 0.000001d;

        public static Vector3? GetTimeAndUvCoord(Vector3 rayOrigin, Vector3 rayDirection, Vector3 vert0, Vector3 vert1, Vector3 vert2)
        {
            var edge1 = vert1 - vert0;
            var edge2 = vert2 - vert0;

            var pvec = Cross(rayDirection, edge2);

            var det = Dot(edge1, pvec);

            if (det > -Epsilon && det < Epsilon)
            {
                return null;
            }

            var invDet = 1d / det;

            var tvec = rayOrigin - vert0;

            var u = Dot(tvec, pvec) * invDet;

            if (u < 0 || u > 1)
            {
                return null;
            }

            var qvec = Cross(tvec, edge1);

            var v = Dot(rayDirection, qvec) * invDet;

            if (v < 0 || u + v > 1)
            {
                return null;
            }

            var t = Dot(edge2, qvec) * invDet;

            return new Vector3((float)t, (float)u, (float)v);
        }

        private static double Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        private static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            Vector3 dest;

            dest.X = v1.Y * v2.Z - v1.Z * v2.Y;
            dest.Y = v1.Z * v2.X - v1.X * v2.Z;
            dest.Z = v1.X * v2.Y - v1.Y * v2.X;

            return dest;
        }

        public static Vector3 GetTrilinearCoordinateOfTheHit(float t, Vector3 rayOrigin, Vector3 rayDirection)
        {
            return rayDirection * t + rayOrigin;
        }

        public static Vector3 UnProject(
          ref Matrix4 projection,
          Matrix4 view,
          System.Drawing.Size viewport,
          Vector3 mouse)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;
            vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
            vec.Z = mouse.Z;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < -float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return new Vector3(vec.X, vec.Y, vec.Z);
        }
        public static Ray MouseRay(
          Matrix4 projection,
          Matrix4 view,
          System.Drawing.Size viewport,
          Vector2 mouse)
        {
            // these mouse.Z values are NOT scientific. 
            // Near plane needs to be < -1.5f or we have trouble selecting objects right in front of the camera. (why?)
            Vector3 pos1 = UnProject(ref projection, view, viewport, new Vector3(mouse.X, mouse.Y, -1.5f)); // near
            Vector3 pos2 = UnProject(ref projection, view, viewport, new Vector3(mouse.X, mouse.Y, 1.0f));  // far
            return new Ray(pos1, pos2);
        }
        public static Ray CamRay(Vivid3D.Scene.GraphCam3D cam,int x,int y)
        {

            return MouseRay(cam.ProjMat, cam.CamWorld, new System.Drawing.Size(Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H), new Vector2(x, y));

        }

    }
}
