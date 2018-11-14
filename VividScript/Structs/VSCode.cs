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

        public override dynamic Exec ( )
        {
            foreach ( VStruct l in Lines )
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
                    case StrandType.If:
                        Lines.Add ( new VSIf ( TokStream ) );
                        break;

                    case StrandType.For:
                        Lines.Add ( new VSFor ( TokStream ) );
                        break;

                    case StrandType.While:
                        //t = ConsumeNext ( );
                        Lines.Add ( new VSWhile ( TokStream ) );
                        //Console.WriteLine ( "while:" + t );

                        break;

                    case StrandType.Assignment:
                        //t = ConsumeNext ( );
                        VSAssign as_line = new VSAssign(TokStream);
                        Lines.Add ( as_line );

                        break;

                    case StrandType.FlatStatement:
                        //t = ConsumeNext ( );

    

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