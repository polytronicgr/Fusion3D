using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VividScript.VStructs
{
    public class VSEntry : VStruct
    {
        public VSEntry(VTokenStream s) : base(s)
        {
            TokStream = s;
        }
        public override void SetupParser()
        {
            Parser = (t) =>
            {
                switch (t.Class)
                {
                    case TokenClass.Define:
                        switch (t.Token) {
                            case Token.Module:
                                Console.WriteLine("Parsing Define.");
                                Console.WriteLine("Module:" + PeekNext().Text);
                                var mod = new VSModule(TokStream);
                                Structs.Add(mod);
                              //  Console.WriteLine("Parsed module name:" + mod.ModuleName);
                                break;
                        }
                        break;
                }
            };
        }
    }
}
