using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSCode : VStruct
    {
        public List<VStruct> Lines = new List<VStruct>();

        public VSCode ( VTokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return ( "CodeBody." );
        }

        public override void SetupParser ( )
        {
            Console.WriteLine ( "Parsing function code-body." );

            PreParser = ( t ) =>
            {
                BackOne ( );
            };

            Parser = ( t ) =>
            {
                Console.WriteLine ( "Code:" + t.Token + " Txt:" + t.Text );

                StrandType strand_type=Predict ( );
                // SkipOne ( );
                switch ( strand_type )
                {
                    case StrandType.FlatStatement:
                        t = ConsumeNext ( );

                        Console.WriteLine ( "Flat Statement. FuncCall:" + t.Token + " TXT:" + t.Text );

                        VToken left_par = ConsumeNext();

                        if ( left_par.Token == Token.LeftPara )
                        {
                            VSFlatCall line = new VSFlatCall ( TokStream )
                            {
                                FuncName = t.Text
                            };
                            Lines.Add ( line );
                        }

                        break;
                }
            };
        }
    }
}