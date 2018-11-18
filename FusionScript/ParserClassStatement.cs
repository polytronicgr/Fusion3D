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

        public StructClassCall ParseClassStatement(ref int i)
        {

            var scc = new StructClassCall();

            for (i = i; i < toks.Len; i++)
            {

                switch (Get(i).Token)
                {
                    case Token.Id:
                        scc.call.Add(Get(i).Text);
                        break;
                    case Token.LeftPara:
                        Log("Checking.", i);
                        var pars = ParseCallPars(ref i);
                        scc.Pars = pars;

                        i = NextToken(i, Token.RightPara);
                        return scc;
                        break;
                }

            }

            return scc;

        }


    }
}
