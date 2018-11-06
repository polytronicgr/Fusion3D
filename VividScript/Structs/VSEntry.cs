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
                //Console.WriteLine("Entry. T:" + t.Class + " Txt:" + t.Text);
                switch (t.Class)
                {
                    case TokenClass.Define:
                        switch (t.Token) {
                            case Token.Func:
                                var name = PeekNext().Text;
                                Console.WriteLine("Func:" + name);
                                var func = new VSFunc(TokStream);
                                Structs.Add(func);
                                break;
                            case Token.Module:
                                Console.WriteLine("Parsing Define.");
                                Console.WriteLine("Module:" + PeekNext().Text);
                                var mod = new VSModule(TokStream);
                                Structs.Add(mod);
                                Console.WriteLine("Parsed module name:" + mod.ModuleName);
                                break;
                        }
                        break;
                }
            };
        }
    }
}
