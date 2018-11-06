using System;

namespace VividScript.VStructs
{
    public class VSCode : VStruct
    {
        public VSCode ( VTokenStream s ) : base ( s )
        {
        }

        public override void SetupParser ( )
        {
            Console.WriteLine ( "Parsing function code-body." );

            PreParser = ( t ) =>
            {
            };

            Parser = ( t ) =>
            {
                Console.WriteLine ( "Code:" + t.Token + " Txt:" + t.Text );
            };
        }
    }
}