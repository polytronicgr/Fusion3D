using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructEntry : Struct
    {
        public StructEntry ( TokenStream s ) : base ( s )
        {
            TokStream = s;
        }

        public List<StructModule> Modules = new List<StructModule>();
        public List<StructFunc> SystemFuncs = new List<StructFunc>();

        public override string DebugString ( )
        {
            return "EntryPoint: Modules:" + Modules.Count + " SysFuncs:" + SystemFuncs.Count;
        }

        public StructFunc FindSystemFunc ( string name )
        {
            foreach ( StructFunc func in SystemFuncs )
            {
                if ( func.FuncName == name )
                {
                    return func;
                }
            }
            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                //Console.WriteLine("Entry. T:" + t.Class + " Txt:" + t.Text);
                if (t.Token == Token.End)
                {
                    t = ConsumeNext();

                }
                while (true)
                {
                    if (t.Token == Token.BeginLine)
                    {
                        t = ConsumeNext();
                    }
                    else
                    {
                        break;
                    }
                }
                switch ( t.Class )
                {
                    case TokenClass.Define:
                        switch ( t.Token )
                        {
                            case Token.Func:
                                string name = PeekNext().Text;
                                Console.WriteLine ( "Func:" + name );
                                StructFunc func = new StructFunc ( TokStream )
                                {
                                    Name = name
                                };
                                Structs.Add ( func );
                                Console.WriteLine ( "Parsed function:" + name );
                                SystemFuncs.Add ( func );
                                break;

                            case Token.Module:
                                //Console.WriteLine ( "Parsing Define.--------------------------------" );
                                Console.WriteLine ( "Module:" + PeekNext ( ).Text );
                                StructModule mod = new StructModule(TokStream);
                                var pn = PeekNext();
                                Structs.Add ( mod );
                                Console.WriteLine ( "Parsed module name:" + mod.ModuleName );
                                Modules.Add ( mod );
                       
                                break;
                        }
                        break;
                }
            };
        }
    }
}