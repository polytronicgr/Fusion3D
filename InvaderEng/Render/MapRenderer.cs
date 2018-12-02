using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvaderEng.Map;
using FusionEngine.Draw;
using OpenTK;
namespace InvaderEng.Render
{
    public class GeoVertex
    {
        public Vector3 Pos;
        public Vector3 Uv;
        public Vector3 Norm;
        public GeoVertex(Vector3 p,Vector3 uv,Vector3 norm)
        {
            Pos = p;
            Uv = uv;
            Norm = norm;
        }
    }
    public class GeoTri
    {
        public int V1, V2, V3;
        public GeoTri(int v1,int v2,int v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
    }
    public class MapGeo
    {
        public GeoVertex[] Vertices;
        public GeoTri[] Tris;
        public MapGeo(int vertices,int tris)
        {
            Vertices = new GeoVertex[vertices];
            Tris = new GeoTri[tris];
        }
    }

    public class MapRenderer
    {

        public Map.Map Map = null;

        public int RenX, RenY;
        public int RenW, RenH;
        public bool GeneratedGeo = false;
        public MapGeo Geo = null;

        public MapRenderer()
        {

        }

        public void GenerateGeo()
        {

            int vc = 0;
            int tc = 0;

            foreach(var e in Map.Elements)
            {
                if(e is MapWall)
                {
                    vc += 4;
                    tc += 2;
                }
            }
            Geo = new MapGeo(vc, tc);
            int vi=0, ti = 0;

            foreach(var e in Map.Elements)
            {

                if(e is MapWall)
                {
                    var mw = e as MapWall;
                    GeoVertex v1, v2, v3, v4;

                    Vector3 n = new Vector3(1, 1, 1);

                    v1 = new GeoVertex(new Vector3(mw.X1, 0, mw.Z1), new Vector3(0, 0, 0), n);
                    v2 = new GeoVertex(new Vector3(mw.X2, 0, mw.Z2), new Vector3(1, 0, 0), n);
                    v3 = new GeoVertex(new Vector3(mw.X2, mw.Height, mw.Z2), new Vector3(1, 1, 0), n);
                    v4 = new GeoVertex(new Vector3(mw.X1, mw.Height, mw.Z1), new Vector3(0, 1, 0), n);

                    GeoTri t1, t2;
                    t1 = new GeoTri(0, 1, 2);
                    t2 = new GeoTri(1, 2, 3);

                    Geo.Vertices[vi++] = v1;
                    Geo.Vertices[vi++] = v2;
                    Geo.Vertices[vi++] = v3;
                    Geo.Vertices[vi++] = v4;

                    Geo.Tris[ti++] = t1;
                    Geo.Tris[ti++] = t2;

                }

            }

            Console.WriteLine("Tris:" + Geo.Tris.Length + " Verts:" + Geo.Vertices.Length);
            GeneratedGeo = true;
        }

        public void Render()
        {
            if (GeneratedGeo == false) GenerateGeo();
            float a = 0;

            float ai = 1.0f / (float)RenW;

            for (int x = 0; x < RenW; x++)
            {

                Pen2D.Rect(RenX + x, RenY, 2, RenH, new OpenTK.Vector4(a, a, a, 1.0f));

                a += ai;
            }

        }

        public struct Line
        {
            public double x1 { get; set; }
            public double y1 { get; set; }

            public double x2 { get; set; }
            public double y2 { get; set; }
        }

        public struct Point
        {
            public double x { get; set; }
            public double y { get; set; }
        }


        //  Returns Point of intersection if do intersect otherwise default Point (null)
        public static Point FindIntersection(Line lineA, Line lineB, double tolerance = 0.001)
        {
            double x1 = lineA.x1, y1 = lineA.y1;
            double x2 = lineA.x2, y2 = lineA.y2;

            double x3 = lineB.x1, y3 = lineB.y1;
            double x4 = lineB.x2, y4 = lineB.y2;

            // equations of the form x = c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
            {
                throw new Exception("Both lines overlap vertically, ambiguous intersection points.");
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
            {
                throw new Exception("Both lines overlap horizontally, ambiguous intersection points.");
            }

            //equations of the form x=c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
            {
                return default(Point);
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
            {
                return default(Point);
            }

            //general equation of line is y = mx + c where m is the slope
            //assume equation of line 1 as y1 = m1x1 + c1 
            //=> -m1x1 + y1 = c1 ----(1)
            //assume equation of line 2 as y2 = m2x2 + c2
            //=> -m2x2 + y2 = c2 -----(2)
            //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
            //so we will get below two equations 
            //-m1x + y = c1 --------(3)
            //-m2x + y = c2 --------(4)

            double x, y;

            //lineA is vertical x1 = x2
            //slope will be infinity
            //so lets derive another solution
            if (Math.Abs(x1 - x2) < tolerance)
            {
                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x1=c1=x
                //subsitute x=x1 in (4) => -m2x1 + y = c2
                // => y = c2 + m2x1 
                x = x1;
                y = c2 + m2 * x1;
            }
            //lineB is vertical x3 = x4
            //slope will be infinity
            //so lets derive another solution
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x3=c3=x
                //subsitute x=x3 in (3) => -m1x3 + y = c1
                // => y = c1 + m1x3 
                x = x3;
                y = c1 + m1 * x3;
            }
            //lineA & lineB are not vertical 
            //(could be horizontal we can handle it with slope = 0)
            else
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                //plugging x value in equation (4) => y = c2 + m2 * x
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                //verify by plugging intersection point (x, y)
                //in orginal equations (1) & (2) to see if they intersect
                //otherwise x,y values will not be finite and will fail this check
                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                    && Math.Abs(-m2 * x + y - c2) < tolerance))
                {
                    return default(Point);
                }
            }

            //x,y can intersect outside the line segment since line is infinitely long
            //so finally check if x, y is within both the line segments
            if (IsInsideLine(lineA, x, y) &&
                IsInsideLine(lineB, x, y))
            {
                return new Point { x = x, y = y };
            }

            //return default null (no intersection)
            return default(Point);

        }

        // Returns true if given point(x,y) is inside the given line segment
        private static bool IsInsideLine(Line line, double x, double y)
        {
            return (x >= line.x1 && x <= line.x2
                        || x >= line.x2 && x <= line.x1)
                   && (y >= line.y1 && y <= line.y2
                        || y >= line.y2 && y <= line.y1);
        }
    }
}
