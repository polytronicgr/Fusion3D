namespace Vivid3D.Composition.Compositers
{
    public class BloomCompositer : Compositer
    {
        public BloomCompositer ( ) : base ( 3 )
        {
            InputFrame = new FrameTypes.FrameColor ( );

            Types [ 0 ] = new FrameTypes.FrameEffect ( );

            Types [ 1 ] = new FrameTypes.FrameEffect ( );

            Types [ 2 ] = new FrameTypes.FrameEffect ( );

            dynamic fe = Types[0];

            fe.FX = new PostProcess.Processes.VEExtract ( );

            fe.FX.MinLevel = 0.8f;

            Types [ 1 ].TexBind.Add ( InputFrame );

            dynamic f2 = Types[1];

            f2.FX = new PostProcess.Processes.VEBlur ( );

            f2.FX.Blur = 2.0f;

            dynamic f3 = Types[2];

            f3.FX = new PostProcess.Processes.VEBloom ( );

            Types [ 1 ].TexBind.Add ( Types [ 1 ] );

            Types [ 2 ].TexBind.Add ( Types [ 1 ] );
            Types [ 2 ].TexBind.Add ( InputFrame );
            //Types[3].TexBind.Add( )

            OutputFrame = Types [ 2 ]; // Types [ 2 ];
            Blend = FrameBlend.Add;
        }

        public override void PreGen ( )
        {
            return;
            Types [ 0 ] = InputFrame;
            Types [ 1 ].TexBind.Clear ( );
            Types [ 1 ].TexBind.Add ( InputFrame );
            Types [ 2 ].TexBind.Clear ( );
            Types [ 2 ].TexBind.Add ( InputFrame );
        }
    }
}