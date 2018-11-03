namespace Vivid3D.Composition.Compositers
{
    public class BloomCompositer : Compositer
    {
        public BloomCompositer ( ) : base ( 4 )
        {
            Types [ 0 ] = new FrameTypes.FrameColor ( );

            Types [ 1 ] = new FrameTypes.FrameEffect ( );

            Types [ 2 ] = new FrameTypes.FrameEffect ( );

            Types [ 3 ] = new FrameTypes.FrameEffect ( );

            dynamic fe = Types[1];

            fe.FX = new PostProcess.Processes.VEExtract ( );

            fe.FX.MinLevel = 0.8f;

            Types [ 1 ].TexBind.Add ( Types [ 0 ] );

            dynamic f2 = Types[2];

            f2.FX = new PostProcess.Processes.VEBlur ( );

            f2.FX.Blur = 2.0f;

            dynamic f3 = Types[3];

            f3.FX = new PostProcess.Processes.VEBloom ( );

            Types [ 2 ].TexBind.Add ( Types [ 1 ] );

            Types [ 3 ].TexBind.Add ( Types [ 2 ] );
            Types [ 3 ].TexBind.Add ( Types [ 0 ] );
            //Types[3].TexBind.Add( )

            OutputFrame = Types [ 3 ];
            Blend = FrameBlend.Add;
        }

        public override void PreGen ( )
        {
            Types [ 0 ] = InputFrame;
            Types [ 1 ].TexBind.Clear ( );
            Types [ 1 ].TexBind.Add ( InputFrame );
            Types [ 2 ].TexBind.Clear ( );
            Types [ 2 ].TexBind.Add ( InputFrame );
        }
    }
}