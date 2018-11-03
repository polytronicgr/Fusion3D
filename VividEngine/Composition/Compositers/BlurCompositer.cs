namespace Vivid3D.Composition.Compositers
{
    public class BlurCompositer : Compositer
    {
        public BlurCompositer ( ) : base ( 2 )
        {
            Types [ 0 ] = new FrameTypes.FrameColor ( );

            Types [ 1 ] = new FrameTypes.FrameEffect ( );

            FrameTypes.FrameEffect fe = Types [ 1 ] as FrameTypes.FrameEffect;

            fe.FX = new PostProcess.Processes.VEBlur ( );

            dynamic bb = fe.FX;

            bb.Blur = 3.0f;

            Types [ 1 ].TexBind.Add ( Types [ 0 ] );

            OutputFrame = Types [ 1 ];

            Blend = FrameBlend.Add;
        }

        public override void PreGen ( )
        {
            return;
            Types [ 0 ] = InputFrame;
            Types [ 1 ].TexBind.Clear ( );
            Types [ 1 ].TexBind.Add ( InputFrame );
            //Types [ 0 ] = InputFrame;
        }
    }
}