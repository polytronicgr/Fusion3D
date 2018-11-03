namespace Vivid3D.Composition.FrameTypes
{
    public class FrameColor : FrameType
    {
        public override void Generate ( )
        {
            BindTarget ( );

            Graph.Render ( );

            ReleaseTarget ( );
        }
    }
}