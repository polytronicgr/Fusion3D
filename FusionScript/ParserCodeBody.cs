using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
using System.IO;
namespace FusionScript
{
    public partial class Parser
    {

        public StructCode ParseCodeBody(ref int i)
        {

            Log("Begun parsing code body", i);



            var code = new StructCode();

            for (i = i; i < toks.Len; i++)
            {
                if(Get(i).Token == Token.End)
                {
                    return code;
                }
                var st = Predict(i);
                switch (st)
                {
                    case StrandType.Assignment:

                        Log("Parsing assignment.", i);

                        i = NextToken(i, Token.Id);

                        var assign = ParseAssign(ref i);

                        code.Lines.Add(assign);

                        break;
                    case StrandType.ClassStatement:
                        Log("Parsing class-statement.", i);
                        i = NextToken(i, Token.Id);

                        var class_state = ParseClassStatement(ref i);

                        code.Lines.Add(class_state);

                        break;
                    case StrandType.FlatStatement:
                        Log("Parsing flat-statement.", i);
                        i = NextToken(i, Token.Id);

                        if(Get(i).Text == "stop")
                        {
                            code.Lines.Add(new StructStop());
                            i++;
                            continue;
                        }

                        var flat_state = ParseFlatStatement(ref i);

                        code.Lines.Add(flat_state);

                        break;
                }


            }


            return code;

        }


    }
}
