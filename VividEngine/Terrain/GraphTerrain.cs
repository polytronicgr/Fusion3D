using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
using Vivid3D.Scene;
using Vivid3D.Texture;
using OpenTK;
namespace Vivid3D.Terrain
{
    public class GraphTerrain : GraphEntity3D
    {

        VMesh TMesh = null;

        Vector3[,] tp = null;
        Vector3[,] tuv = null;
        Vector3[,] tnorm = null;
        Vector3[,] tbi = null;
        Vector3[,] tan = null;
        int[] indices = null;

        public GraphTerrain(float w,float h,float y,int xsegs=32,int ysegs=32)
        {

            int vc = 0;

            tp = new Vector3[xsegs,ysegs];
            tuv = new Vector3[xsegs, ysegs];
            tnorm = new Vector3[xsegs, ysegs];
            tbi = new Vector3[xsegs, ysegs];
            tan = new Vector3[xsegs, ysegs];

            List<int> indices = new List<int>();

            float vx = -w / 2.0f;
            float vz = -h / 2.0f;
            float vy = y;

            float xi = w / (float)xsegs;
            float zi = h / (float)ysegs;

            float ui, vi;

            ui = 0.1f;
            vi = 0.1f;

            float vu, vv;

            vu = 0.0f;
            vv = 0.0f;




            for (int sy = 0; sy < ysegs; sy++)
            {
                for (int sx = 0; sx < xsegs; sx++)
                {

                    tp[sx,sy] = new Vector3(vx, vy, vz);
                    tuv[sx, sy] = new Vector3(vu, vv,0);
                    tnorm[sx,sy] = new Vector3(0, 1, 0);

                    vu += ui;
                    //vv += vi;

                    vx += xi;


                }
                vu = 0;
                vv += vi;
                vx = -w / 2.0f;
                vz += zi;
            }

            
            for(int sy = 0; sy < ysegs-1; sy++)
            {
                for(int sx = 0; sx < xsegs-1; sx++)
                {

                    indices.Add(GetI(sx, sy, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy , xsegs, ysegs));

                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx, sy , xsegs, ysegs));
                    indices.Add(GetI(sx, sy+1, xsegs, ysegs));

                }
            }

            TMesh = new VMesh(indices.Count, xsegs * ysegs);

            int vid = 0;

            for(int sy = 0; sy < ysegs; sy++)
            {
                for(int sx = 0; sx < xsegs; sx++)
                {

                    TMesh.SetVertex(vid, tp[sx, sy], tan[sx, sy], tbi[sx, sy], tnorm[sx, sy], new Vector2(tuv[sx, sy].X, tuv[sx, sy].Y));
                    vid++;
                }
            }

            uint[] mi = new uint[indices.Count];

            for(int i = 0; i < indices.Count; i++)
            {
                mi[i] = (uint)indices[i];
            }

            TMesh.Indices = mi;

            TMesh.Mat = new Material.Material3D();

            TMesh.GenerateTangents();

            TMesh.Final();


            Meshes.Add(TMesh);
            this.Renderer = new Visuals.VRTerrain();
            Name = "Terrain";



        }

        public int GetI(int x,int y,int xs,int ys)
        {
            return y * xs + x;
        }


    }
}
