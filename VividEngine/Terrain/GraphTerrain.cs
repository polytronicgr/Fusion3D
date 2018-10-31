using OpenTK;
using System.Collections.Generic;
using Vivid3D.Data;
using Vivid3D.Scene;

namespace Vivid3D.Terrain
{
    public class GraphTerrain : GraphEntity3D
    {
        private VMesh TMesh = null;

        private Vector3[,] tp = null;
        private Vector3[,] tuv = null;
        private Vector3[,] tnorm = null;
        private Vector3[,] tbi = null;
        private Vector3[,] tan = null;
        private int[] indices = null;

        public GraphTerrain(float w, float h, float y, int xsegs = 32, int ysegs = 32)
        {
            GenFlat(w, h, y, xsegs, ysegs);
        }

        public GraphTerrain(float w, float h, float y, int xsegs, int ysegs, Texture.VTex2D tex)
        {
            float xr = (float)tex.W / w;
            float yr = (float)tex.H / h;

            byte[] data = tex.RawData;

            int vc = 0;

            tp = new Vector3[xsegs, ysegs];
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
                    float ny = 0.0f;

                    float ax = w / 2 + vx;
                    float ay = h / 2 + vz;

                    float tx = ax * xr;
                    float ty = ay * yr;

                    // int loc = (int)((float)ty * (float)tex.W * 3)+ (tx * 3);

                    int loc = (int)ty * (tex.W * 3);
                    loc += (int)tx * 3;

                    ny = data[loc];

                    ny = ny * 3;

                    tp[sx, sy] = new Vector3(vx, vy + ny, vz);
                    tuv[sx, sy] = new Vector3(vu, vv, 0);
                    tnorm[sx, sy] = new Vector3(0, 1, 0);

                    vu += ui;
                    //vv += vi;

                    vx += xi;
                }
                vu = 0;
                vv += vi;
                vx = -w / 2.0f;
                vz += zi;
            }

            for (int sy = 0; sy < ysegs - 1; sy++)
            {
                for (int sx = 0; sx < xsegs - 1; sx++)
                {
                    indices.Add(GetI(sx, sy, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy, xsegs, ysegs));

                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx, sy, xsegs, ysegs));
                    indices.Add(GetI(sx, sy + 1, xsegs, ysegs));
                }
            }

            TMesh = new VMesh(indices.Count, xsegs * ysegs);

            int vid = 0;

            for (int sy = 0; sy < ysegs; sy++)
            {
                for (int sx = 0; sx < xsegs; sx++)
                {
                    TMesh.SetVertex(vid, tp[sx, sy], tan[sx, sy], tbi[sx, sy], tnorm[sx, sy], new Vector2(tuv[sx, sy].X, tuv[sx, sy].Y));
                    vid++;
                }
            }

            uint[] mi = new uint[indices.Count];

            for (int i = 0; i < indices.Count; i++)
            {
                mi[i] = (uint)indices[i];
            }

            for (int i = 0; i < indices.Count / 3; i++)
            {
                TMesh.SetTri(i, indices[i * 3], indices[i * 3 + 1], indices[i * 3 + 2]);
            }

            TMesh.Indices = mi;

            TMesh.Mat = new Material.Material3D();

            TMesh.GenerateTangents();

            TMesh.Final();

            Meshes.Add(TMesh);
            this.Renderer = new Visuals.VRTerrain();
            Name = "Terrain";
        }

        public void GenMapped(float w, float h, float y, int xsegs, int ysegs, Texture.VTex2D tex)
        {
        }

        private void GenFlat(float w, float h, float y, int xsegs, int ysegs)
        {
            int vc = 0;

            tp = new Vector3[xsegs, ysegs];
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
                    tp[sx, sy] = new Vector3(vx, vy, vz);
                    tuv[sx, sy] = new Vector3(vu, vv, 0);
                    tnorm[sx, sy] = new Vector3(0, 1, 0);

                    vu += ui;
                    //vv += vi;

                    vx += xi;
                }
                vu = 0;
                vv += vi;
                vx = -w / 2.0f;
                vz += zi;
            }

            for (int sy = 0; sy < ysegs - 1; sy++)
            {
                for (int sx = 0; sx < xsegs - 1; sx++)
                {
                    indices.Add(GetI(sx, sy, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx + 1, sy, xsegs, ysegs));

                    indices.Add(GetI(sx + 1, sy + 1, xsegs, ysegs));
                    indices.Add(GetI(sx, sy, xsegs, ysegs));
                    indices.Add(GetI(sx, sy + 1, xsegs, ysegs));
                }
            }

            TMesh = new VMesh(indices.Count, xsegs * ysegs);

            int vid = 0;

            for (int sy = 0; sy < ysegs; sy++)
            {
                for (int sx = 0; sx < xsegs; sx++)
                {
                    TMesh.SetVertex(vid, tp[sx, sy], tan[sx, sy], tbi[sx, sy], tnorm[sx, sy], new Vector2(tuv[sx, sy].X, tuv[sx, sy].Y));
                    vid++;
                }
            }

            uint[] mi = new uint[indices.Count];

            for (int i = 0; i < indices.Count; i++)
            {
                mi[i] = (uint)indices[i];
            }

            for (int i = 0; i < indices.Count / 3; i++)
            {
                TMesh.SetTri(i, indices[i * 3], indices[i * 3 + 1], indices[i * 3 + 2]);
            }

            TMesh.Indices = mi;

            TMesh.Mat = new Material.Material3D();

            TMesh.GenerateTangents();

            TMesh.Final();

            Meshes.Add(TMesh);
            this.Renderer = new Visuals.VRTerrain();
            Name = "Terrain";
        }

        public int GetI(int x, int y, int xs, int ys)
        {
            return y * xs + x;
        }
    }
}