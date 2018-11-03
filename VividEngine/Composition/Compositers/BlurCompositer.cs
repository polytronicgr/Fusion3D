namespace Vivid3D.Composition.Compositers
{
    public class BlurCompositer : Compositer
    {
        public BlurCompositer ( ) : base ( 1 )
        {
            InputFrame = new FrameTypes.FrameColor ( );

            Types [ 0 ] = new FrameTypes.FrameEffect ( );

            FrameTypes.FrameEffect fe = Types [ 0 ] as FrameTypes.FrameEffect;

            fe.FX = new PostProcess.Processes.VEBlur ( );

            dynamic bb = fe.FX;

            bb.Blur = 2f;

            Types [ 0 ].TexBind.Add ( InputFrame );

            OutputFrame = Types [ 0 ];

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