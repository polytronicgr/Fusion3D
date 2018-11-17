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

                        i = NextToken(i, Token.Id);

                        var assign = ParseAssign(ref i);

                        code.Lines.Add(assign);

                        break;
                    case StrandType.FlatStatement:

                        i = NextToken(i, Token.Id);

                        var flat_state = ParseFlatStatement(ref i);

                        code.Lines.Add(flat_state);

                        break;
                }


            }


            return code;

        }


    }
}
