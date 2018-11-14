using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructDefineVars : Struct
    {
        public VarType Type = VarType.Bool;
        public List<Var> Vars = new List<Var>();

        public StructDefineVars ( TokenStream s ) : base ( s )
        {
        }

        public override void SetupParser ( )
        {
            PreParser = ( t ) =>
            {
                //Console.WriteLine("T=" + t.ToString());
                switch ( t.Token )
                {
                    case Token.Int:
                        Type = VarType.Int;
                        break;

                    case Token.Byte:
                        Type = VarType.Byte;
                        break;

                    case Token.Short:
                        Type = VarType.Short;
                        break;

                    case Token.Long:
                        Type = VarType.Long;
                        break;

                    case Token.Float:
                        Type = VarType.Float;
                        break;

                    case Token.Double:
                        Type = VarType.Double;
                        break;

                    case Token.String:
                        Type = VarType.String;
                        break;
                }
                Console.WriteLine ( "VarType:" + Type.ToString ( ) );
            };
            Parser = ( t ) =>
            {
                //Console.WriteLine("T==" + t.Text+" Tok:"+t.Token);
                switch ( t.Token )
                {
                    case Token.Id:

                        Var v = new Var
                        {
                            Name = t.Text,
                            Type = Type
                        };
                        Console.WriteLine ( "VAR:" + v.Name + " TYPE:" + v.Type );
                        Vars.Add ( v );
                        break;

                    case Token.EndLine:
                        Done = true;
                        break;
                }
            };
        }
    }
}