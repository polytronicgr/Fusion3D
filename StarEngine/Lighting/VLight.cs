using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using OpenTK;
using Vivid3D.Settings;
using OpenTK.Graphics.OpenGL4;
using Vivid3D.Texture;
using Vivid3D.FrameBuffer;
namespace Vivid3D.Lighting
{
    public enum LightType
    {
        Ambient,Directional,Point
    }
    public class GraphLight3D : GraphNode3D
    {
        public static GraphLight3D Active = null;
        public bool CastShadows = true;
        public LightType Type = LightType.Point;
        public Vector3 Diff = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Spec = new Vector3(0.8f, 0.8f, 1.2f);
        public Vector3 Amb = new Vector3(0, 0, 0);
        public float Atten = 0.002f;
        public float AmbCE = 0.005f;
        public Texture.VTexCube ShadowMap = null;
        public VFrameBufferCube ShadowFB = null;
        public float Range = 100.0f;
        public GraphLight3D()
        {
            //    ShadowMap = new Texture.VTexCube(Quality.ShadowMapWidth, Quality.ShadowMapHeight);
            CreateShadowFBO();
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

        private static void SetCam(TextureTarget f,GraphCam3D Cam)
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
                    Cam.LookAtZero(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
                    break;
                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero(new Vector3(0, -1, 0), new Vector3(0, 0, -1));
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
