using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Fusion3D.FrameBuffer;
using Fusion3D.Scene;

namespace Fusion3D.Enviro
{
    public class EnvRenderer
    {
        public SceneGraph3D Scene;
        public Cam3D Cam = new Cam3D();
        public Vector3 Pos = Vector3.Zero;
        public FrameBufferCube FB = null;

        public EnvRenderer ( int w, int h )
        {
            FB = new FrameBufferCube ( w, h );
        }

        public void Render ( )
        {
            Cam.FOV = 90;
            Cam.Pos ( Pos, Space.Local );

            for ( int i = 0; i < 6; i++ )
            {
                DrawFace ( i );
            }
        }

        private void DrawFace ( int i )
        {
            TextureTarget face = FB.SetFace(i);

            GL.Clear ( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            switch ( face )
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    Cam.LookAtZero ( new Vector3 ( 1, 0, 0 ), new Vector3 ( 0, 1, 0 ) );
                    break;

                case TextureTarget.TextureCubeMapNegativeX:
                    Cam.LookAtZero ( new Vector3 ( -1, 0, 0 ), new Vector3 ( 0, 1, 0 ) );
                    break;

                case TextureTarget.TextureCubeMapPositiveY:
                    Cam.LookAtZero ( new Vector3 ( 0, 10, 0 ), new Vector3 ( 1, 0, 0 ) );
                    break;

                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero ( new Vector3 ( 0, -10, 0 ), new Vector3 ( 1, 0, 0 ) );
                    break;

                case TextureTarget.TextureCubeMapPositiveZ:
                    Cam.LookAtZero ( new Vector3 ( 0, 0, 10 ), new Vector3 ( 0, 1, 0 ) );
                    break;

                case TextureTarget.TextureCubeMapNegativeZ:
                    Cam.LookAtZero ( new Vector3 ( 0, 0, -10 ), new Vector3 ( 0, 1, 0 ) );
                    break;
            }
            Scene.CamOverride = Cam;
            Scene.Render ( );
            Scene.CamOverride = null;
            FB.Release ( );
        }
    }
}