namespace Fusion3D.Composition.FrameTypes
{
    public class FrameDepth : FrameType
    {
        public override void Generate ( )
        {
            BindTarget ( );

            Graph.RenderDepth ( );

            ReleaseTarget ( );
        }
    }
}