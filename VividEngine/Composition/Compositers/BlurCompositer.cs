namespace Vivid3D.Composition.Compositers
{
    public class BlurCompositer : Compositer
    {
        public BlurCompositer ( ) : base ( 1 )
        {
            Types [ 0 ] = new FrameTypes.FrameColor ( );
        }

        public override void Render ( )
        {
            GenerateFrames ( );

            PresentFrame ( 0 );
        }
    }
}