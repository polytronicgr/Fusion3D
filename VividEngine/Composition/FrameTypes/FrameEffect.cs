namespace Vivid3D.Composition.FrameTypes
{
    public class FrameEffect : FrameType
    {
        public Effect.Effect3D FX;

        public override void Generate ( )
        {
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