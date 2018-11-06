using System;

namespace VividScript.VStructs
{
    public class VSEntry : VStruct
    {
        public VSEntry ( VTokenStream s ) : base ( s )
        {
            TokStream = s;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                //Console.WriteLine("Entry. T:" + t.Class + " Txt:" + t.Text);
                switch ( t.Class )
                {
                    case TokenClass.Define:
                        switch ( t.Token )
                        {
                            case Token.Func:
                                string name = PeekNext().Text;
                                Console.WriteLine ( "Func:" + name );
                                VSFunc func = new VSFunc(TokStream);
                                Structs.Add ( func );
                                Console.WriteLine ( "Parsed function:" + name );
                                break;

                            case Token.Module:
                                Console.WriteLine ( "Parsing Define." );
                                Console.WriteLine ( "Module:" + PeekNext ( ).Text );
                                VSModule mod = new VSModule(TokStream);
                                Structs.Add ( mod );
                                Console.WriteLine ( "Parsed module name:" + mod.ModuleName );
                                break;
                        }
                        break;
                }
            };
        }
    }
}