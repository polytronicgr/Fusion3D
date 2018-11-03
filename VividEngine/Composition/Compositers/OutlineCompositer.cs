namespace Vivid3D.Composition.Compositers
{
    public class OutlineCompositer : Compositer
    {
        public OutlineCompositer ( ) : base ( 3 )
        {
            Types [ 0 ] = new FrameTypes.FrameColor ( );

            Types [ 1 ] = new FrameTypes.FrameDepth ( );

            Types [ 2 ] = new FrameTypes.FrameEffect ( );

            dynamic fe = Types[2];

            fe.FX = new PostProcess.Processes.VEOutLine ( );

            Types [ 2 ].TexBind.Add ( Types [ 1 ] );
            Types [ 2 ].TexBind.Add ( Types [ 0 ] );

            OutputFrame = Types [ 2 ];

            Blend = FrameBlend.Add;
        }

        public override void PreGen ( )
        {
            Types [ 0 ] = InputFrame;
            Types [ 2 ].TexBind.Clear ( );
            Types [ 2 ].TexBind.Add ( Types [ 1 ] );
            Types [ 2 ].TexBind.Add ( InputFrame );
        }
    }
}