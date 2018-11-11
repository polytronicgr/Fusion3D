namespace Vivid3D.Composition.FrameTypes
{
    public class FrameEffect : FrameType
    {
        public Effect.Effect3D FX;

        public override void Generate ( )
        {
            OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Blend);
            BindTex ( );

            FX.Bind ( );

            BindTarget ( );

            DrawQuad ( );

            ReleaseTarget ( );

            FX.Release ( );

            ReleaseTex ( );
        }
    }
}