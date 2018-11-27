namespace FusionEngine.Composition.Compositers
{
    public class OutlineCompositer : Compositer
    {
        public OutlineCompositer ( ) : base ( 2 )
        {
            InputFrame = new FrameTypes.FrameColor ( );

            Types [ 0 ] = new FrameTypes.FrameDepth ( );

            Types [ 1 ] = new FrameTypes.FrameEffect ( );

            dynamic fe = Types[1];

            fe.FX = new PostProcess.Processes.VEOutLine ( );

            Types [ 1 ].TexBind.Add ( Types [ 0 ] );
            Types [ 1 ].TexBind.Add ( InputFrame );

            OutputFrame = Types [ 1 ];

            Blend = FrameBlend.Add;
        }

        public override void PreGen ( )
        {
            return;
            Types [ 0 ] = InputFrame;
            Types [ 2 ].TexBind.Clear ( );
            Types [ 2 ].TexBind.Add ( Types [ 1 ] );
            Types [ 2 ].TexBind.Add ( InputFrame );
        }
    }
}