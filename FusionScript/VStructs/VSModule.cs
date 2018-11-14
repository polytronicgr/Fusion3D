using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VividScript.VStructs
{
    public class VSModule : VStruct
    {
        public string ModuleName = "";
        public VSModule(VTokenStream s) : base(s)
        {

        }
        public override void SetupParser()
        {

            PreParser = (t) =>
            {
                ModuleName = t.Text;
             
            };
            Parser = (t) =>
            {

                switch(t.Class)
                {
                    case TokenClass.Type:

                        Console.WriteLine("Parsing Variable definitions.");
                        BackOne();
                        var vdef = new VSDefineVars(TokStream);
                        Structs.Add(vdef);

                        break;
                }
            };
        }
    }
}
