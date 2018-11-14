using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VividScript.VStructs
{
    public class VSDefineVars : VStruct
    {
        public Token Type;
        public VSDefineVars(VTokenStream s) : base(s)
        {

        }
        public override void SetupParser()
        {
            PreParser = (t) =>
            {
                Console.WriteLine("T=" + t.ToString());
                switch (t.Token)
                {
                    case Token.Int:
                        Type = Token.Int;
                        break;
                    case Token.Byte:
                        Type = Token.Byte;
                        break;
                    case Token.Short:
                        Type = Token.Short;
                        break;
                    case Token.Long:
                        Type = Token.Long;
                        break;
                    case Token.Float:
                        Type = Token.Float;
                        break;
                    case Token.Double:
                        Type = Token.Double;
                        break;
                    case Token.String:
                        Type = Token.String;
                        break;
                }
                Console.WriteLine("VarType:" + Type.ToString());
            };
            Parser = (t) =>
            {
                switch (t.Token)
                {
                    case Token.Id:
                        Console.WriteLine("Var:" + t.Text);
                        break;
                }
            };
        }
    }
}
