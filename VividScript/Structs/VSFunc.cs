using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VividScript.VStructs
{
    public class VSFunc : VStruct
    {
        public string FuncName = "";
        public VSFunc(VTokenStream s) : base(s)
        {

        }
        public override void SetupParser()
        {
            PreParser = (t) =>
            {
                FuncName = t.Text;
            };

            Parser = (t) =>
            {
                if (t.Token == Token.End)
                {
                    Done = true;
                    return;
                }
            };
        }
    }
}
