using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid3D.FrameBuffer;
using Vivid3D.Scene;
using Vivid3D.Settings;

namespace Vivid3D.Lighting
{
    public enum LightType
    {
        Ambient, Directional, Point
    }

    public class GraphLight3D : GraphNode3D
    {
        public GraphLight3D()
        {
            Diff = new Vector3(0.6f, 0.6f, 0.6f);
            Spec = new Vector3(0.2f, 0.2f, 0.2f);
            Amb = new Vector3(0.1f, 0.1f, 0.1f);
            Atten = 0.1f;
            CreateShadowFBO();
            LightNum++;
            Range = 5;
        }

        public static GraphLight3D Active = null;
        public bool CastShadows = true;
        public LightType Type = LightType.Point;

        public Vector3 Diff
        {
            get;
            set;
        }

        public Vector3 Spec
        {
            get;
            set;
        }

        public Vector3 Amb
        {
            get;
            set;
        }

        public static int LightNum = 0;

        public float Atten
        {
            get;
            set;
        }

        //   public Vector3 AmbCE = 0.3f;
        public Texture.VTexCube ShadowMap = null;

        public VFrameBufferCube ShadowFB = null;

        public float Range
        {
            get;
            set;
        }

        public void Write()
        {
            Help.IOHelp.WriteMatrix(LocalTurn);
            Help.IOHelp.WriteVec(LocalPos);
            Help.IOHelp.WriteVec(Diff);
            Help.IOHelp.WriteVec(Spec);
            Help.IOHelp.WriteVec(Amb);
            Help.IOHelp.WriteFloat(Range);
            Help.IOHelp.WriteBool(CastShadows);
        }

        public void Read()
        {
            LocalTurn = Help.IOHelp.ReadMatrix();
            LocalPos = Help.IOHelp.ReadVec3();
            Diff = Help.IOHelp.ReadVec3();
            Spec = Help.IOHelp.ReadVec3();
            Amb = Help.IOHelp.ReadVec3();
            Range = Help.IOHelp.ReadFloat();
            CastShadows = Help.IOHelp.ReadBool();
        }

        public void CreateShadowFBO()
        {
            ShadowFB = new VFrameBufferCube(Quality.ShadowMapWidth, Quality.ShadowMapHeight);
        }

        public void DrawShadowMap(SceneGraph3D graph)
        {
            Active = this;

            GraphCam3D cam = new GraphCam3D();
            Effect.FXG.Cam = cam;
            cam.FOV = 90;
            cam.MaxZ = Quality.ShadowDepth;
            Effect.FXG.Proj = cam.ProjMat;

            graph.CamOverride = cam;
            cam.LocalPos = LocalPos;
            cam.MaxZ = Quality.ShadowDepth;
            //  cam.LocalTurn = LocalTurn;

            int fn = 0;

            var f = ShadowFB.SetFace(fn);
            SetCam(f, cam);

            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(1), cam);
            graph.RenderDepth();

            // ShadowFB.Release();
            //  graph.CamOverride = null;

            SetCam(ShadowFB.SetFace(2), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(3), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(4), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(5), cam);
            graph.RenderDepth();

            ShadowFB.Release();

            graph.CamOverride = null;
        }

        private static void SetCam(TextureTarget f, GraphCam3D Cam)
        {
            switch (f)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    Cam.LookAtZero(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeX:
                    Cam.LookAtZero(new Vector3(-1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapPositiveY:

                    Cam.LookAtZero(new Vector3(0, -1, 0), new Vector3(0, 0, -1));
                    break;

                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
                    break;

                case TextureTarget.TextureCubeMapPositiveZ:
                    Cam.LookAtZero(new Vector3(0, 0, 1), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeZ:
                    Cam.LookAtZero(new Vector3(0, 0, -1), new Vector3(0, -1, 0));
                    break;
            }
        }
    }
}