using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using OpenTK;
using OpenTK.Graphics;
using Vivid3D.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.Enviro
{
    public class VEnvRenderer
    {
        public SceneGraph3D Scene;
        public GraphCam3D Cam = new GraphCam3D();
        public Vector3 Pos = Vector3.Zero;
        public VFrameBufferCube FB = null;
        public VEnvRenderer(int w,int h)
        {
            FB = new VFrameBufferCube(w, h);

        }
        public void Render()
        {
            Cam.FOV = 90;
            Cam.Pos(Pos, Space.Local);
            
            for (int i = 0; i < 6; i++)
            {
                DrawFace(i);
            }
        }

        private void DrawFace(int i)
        {
            var face = FB.SetFace(i);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            switch (face)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    Cam.LookAtZero(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
                    break;
                case TextureTarget.TextureCubeMapNegativeX:
                    Cam.LookAtZero(new Vector3(-1, 0, 0), new Vector3(0, 1, 0));
                    break;
                case TextureTarget.TextureCubeMapPositiveY:
                    Cam.LookAtZero(new Vector3(0, 10, 0), new Vector3(1, 0, 0));
                    break;
                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero(new Vector3(0, -10, 0), new Vector3(1, 0, 0));
                    break;
                case TextureTarget.TextureCubeMapPositiveZ:
                    Cam.LookAtZero(new Vector3(0, 0, 10), new Vector3(0, 1, 0));
                    break;
                case TextureTarget.TextureCubeMapNegativeZ:
                    Cam.LookAtZero(new Vector3(0, 0, -10), new Vector3(0, 1, 0));
                    break;
            }
            Scene.CamOverride = Cam;
            Scene.Render();
            Scene.CamOverride = null;
            FB.Release();
        }
    }
}
