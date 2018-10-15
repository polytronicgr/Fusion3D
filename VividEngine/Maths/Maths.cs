using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Vivid3D.Util
{
    public static class Maths
    {
        public static float DegToRad(float d)
        {
            return (float)Math.PI * d / 180.0f;
        }
        public static float RadToDeg(float r)
        {
            return r * (180.0f / (float)Math.PI);
        }
        public static float Cos(float d)
        {
            return (float)Math.Cos(DegToRad(d));
        }
        public static float Sin(float d)
        {
            return (float)Math.Sin(DegToRad(d));
        }
        public static Vector2 Push(Vector2 p,float x,float y)
        {
            return new Vector2(p.X + x, p.Y + y);
        }
        public static Vector2[] Push(Vector2[] p, float x, float y, float scale = 1.0f)
        {

            for (int i = 0; i < 4; i++)
            {
                p[i].X = p[i].X + x * scale;
                p[i].Y = p[i].Y + y * scale;
            }
            return p;

        }
        public static Vector2 Point(float x, float y)
        {
            return new Vector2(x, y);
        }

        public static float Distance(Vector2 p1, Vector2 p2)
        {
            Vector2 p3 = p2 - p1;
            return p3.Length;
        }

     

        public static void Rotate(ref Vector2 p, ref float a, ref Vector2 res)
        {
            res.X = p.X * Cos(a) - p.Y * Sin(a);
            res.Y = p.X * Sin(a) + p.Y * Cos(a);
        }
        public static Vector2[] Rotate(Vector2[] p, float a, float s = 1.0f)
        {
            for (int i = 0; i < 4; i++)
            {
                p[i] = Rotate(p[i].X, p[i].Y, a, s);
            }
            return p;
        }
        public static Vector2 Rotate(ref Vector2 p, ref float a)
        {
            var res = new Vector2();
            Rotate(ref p, ref a, ref res);
            return res;
        }

        public static void Scale(ref Vector2 p, ref float s, ref Vector2 res)
        {
            res.X = p.X * s;
            res.Y = p.Y * s;
        }

        public static Vector2 Scale(ref Vector2 p, ref float s)
        {
            var res = new Vector2();
            Scale(ref p, ref s, ref res);
            return res;
        }

        public static void Transform2D(ref Vector2 p, ref Vector2 origin, ref float a, ref float s, ref Vector2 res)
        {
            Vector2 np = p - origin;
            Vector2 nr = Rotate(ref np, ref a);
            Vector2 ns = Scale(ref nr, ref s);
            res = ns + origin;
        }

        public static void GetOrigin(ref Vector4 rect, ref Vector2 res)
        {
            res.X = rect.X + rect.Z * 0.5f;
            res.Y = rect.Y + rect.W * 0.5f;
        }

        public static Vector2 GetOrigin(ref Vector4 rect)
        {
            var res = new Vector2();
            GetOrigin(ref rect, ref res);
            return res;
        }

        public static void TransformRect(ref Vector4 rect, ref Vector2[] res, ref float a, ref float s)
        {
            var o = new Vector2();
            GetOrigin(ref rect, ref o);

            Transform2D(new Vector2(rect.X, rect.Y), ref o, ref a, ref s, ref res[0]);
            Transform2D(new Vector2(rect.X + rect.Z, rect.Y), ref o, ref a, ref s, ref res[1]);
            Transform2D(new Vector2(rect.X + rect.Z, rect.Y + rect.W), ref o, ref a, ref s, ref res[2]);
            Transform2D(new Vector2(rect.X, rect.Y + rect.W), ref o, ref a, ref s, ref res[3]);
        }

        public static Vector2[] TransformRect(ref Vector4 rect, ref float a, ref float s)
        {
            var res = new Vector2[4];
            res[0] = new Vector2();
            res[1] = new Vector2();
            res[2] = new Vector2();
            res[3] = new Vector2();
            TransformRect(ref rect, ref res, ref a, ref s);
            return res;
        }

        public static void Transform2D(Vector2 pos, ref Vector2 origin, ref float a, ref float s, ref Vector2 res)
        {
            Transform2D(ref pos, ref origin, ref a, ref s, ref res);
        }

        public static Vector2 Transform2D(ref Vector2 p, ref Vector2 origin, ref float a, ref float s)
        {
            var res = new Vector2();
            Transform2D(ref p, ref origin, ref a, ref s, ref res);
            return res;
        }

        public static Vector2[] RotateOC(Vector2[] p, float rot, float scale, float xo, float yo)
        {
            Vector2[] res = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                res[i] = RotateOC(p[i].X, p[i].Y, rot, scale, xo, yo);
            }
            return res;
        }
        public static Vector2[] RotateOC(float[] x, float[] y, float rot, float scale, float xo, float yo)
        {
            Vector2[] res = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                res[i] = RotateOC(x[i], y[i], rot, scale, xo, yo);

            }
            return res;
        }

        public static Vector2 RotateOC(float x, float y, float rot, float scale, float xo, float yo)
        {

            Vector2 res;

            res = Rotate(x, y, rot, scale);

            return res;

        }
        public static Vector2 Rotate(float x, float y, float rot, float scale)
        {
            Vector2 res = new Vector2();

            float rr = rot * (float)Math.PI / 180.0f;

            res.X = ((float)Math.Cos(rr) * x - (float)Math.Sin(rr) * y);
            res.Y = ((float)Math.Sin(rr) * x + (float)Math.Cos(rr) * y);

            res.X = res.X * scale;
            res.Y = res.Y * scale;

            return res;

        }

    }
}