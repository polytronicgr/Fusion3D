using System;

namespace FusionScript.Structs
{
    public class StructFunc : Struct
    {
        public string FuncName = "";
        public StructPars Pars = null;
        public StructCode Code = null;
        public bool Static = false;
 

        public override string DebugString ( )
        {
            return "Func:" + FuncName;
        }

        public override void SetupParser ( )
        {
            PreParser = ( t ) =>
            {
                FuncName = t.Text;
            };

            Parser = ( t ) =>
            {
           
            };
        }
    }
}