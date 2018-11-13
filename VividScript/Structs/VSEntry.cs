using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSEntry : VStruct
    {
        public VSEntry ( VTokenStream s ) : base ( s )
        {
            TokStream = s;
        }

        public List<VSModule> Modules = new List<VSModule>();
        public List<VSFunc> SystemFuncs = new List<VSFunc>();

        public override string DebugString ( )
        {
            return "EntryPoint: Modules:" + Modules.Count + " SysFuncs:" + SystemFuncs.Count;
        }

        public VSFunc FindSystemFunc ( string name )
        {
            foreach ( VSFunc func in SystemFuncs )
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
                switch ( t.Class )
                {
                    case TokenClass.Define:
                        switch ( t.Token )
                        {
                            case Token.Func:
                                string name = PeekNext().Text;
                                Console.WriteLine ( "Func:" + name );
                                VSFunc func = new VSFunc ( TokStream )
                                {
                                    Name = name
                                };
                                Structs.Add ( func );
                                Console.WriteLine ( "Parsed function:" + name );
                                SystemFuncs.Add ( func );
                                break;

                            case Token.Module:
                                Console.WriteLine ( "Parsing Define.--------------------------------" );
                                Console.WriteLine ( "Module:" + PeekNext ( ).Text );
                                VSModule mod = new VSModule(TokStream);
                                Structs.Add ( mod );
                                Console.WriteLine ( "Parsed module name:" + mod.ModuleName );
                                Modules.Add ( mod );
                                Console.WriteLine("NextTok:" + PeekNext());
                                break;
                        }
                        break;
                }
            };
        }
    }
}