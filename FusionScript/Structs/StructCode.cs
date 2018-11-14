using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructCode : Struct
    {
        public List<Struct> Lines = new List<Struct>();

        public StructCode ( TokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return ( "CodeBody." );
        }

        public override dynamic Exec ( )
        {
            foreach ( Struct l in Lines )
            {
                l.Exec ( );
            }
            return null;
        }

        public override void SetupParser ( )
        {
   

            PreParser = ( t ) =>
            {
                BackOne ( );
            };

            Parser = ( t ) =>
            {
              
                if(t.Token == Token.BeginLine)
                {
                    return;
                }
                if ( t.Token == Token.EndLine )
                {
                    return;
                }
                if ( t.Token == Token.EndLine )
                {
                    return;
                }
                if ( t.Token == Token.End )
                {
                    Done = true;
                    return;
                }
                StrandType strand_type=Predict ( );
                // SkipOne ( );
                switch ( strand_type )
                {
                    case StrandType.ClassStatement:
                        Console.WriteLine("ClassCall");
                        BackOne();
                        Lines.Add(new StructClassCall(TokStream));
                        Console.WriteLine("::");

                        break;
                    case StrandType.If:
                        Lines.Add ( new StructIf ( TokStream ) );
                        break;

                    case StrandType.For:
                        Lines.Add ( new StructFor ( TokStream ) );
                        break;

                    case StrandType.While:
                        //t = ConsumeNext ( );
                        Lines.Add ( new StructWhile ( TokStream ) );
                        //Console.WriteLine ( "while:" + t );

                        break;

                    case StrandType.Assignment:
                        //t = ConsumeNext ( );
                        StructAssign as_line = new StructAssign(TokStream);
                        Lines.Add ( as_line );

                        break;

                    case StrandType.FlatStatement:
                        //t = ConsumeNext ( );

    

                        CodeToken left_par = ConsumeNext();

                        if ( left_par.Token == Token.LeftPara )
                        {
                            StructFlatCall line = new StructFlatCall ( TokStream )
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