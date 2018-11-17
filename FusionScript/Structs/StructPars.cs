using System;

namespace FusionScript.Structs
{
    public class StructPars : Struct
    {
        public System.Collections.Generic.List<Var> Pars = new System.Collections.Generic.List<Var>();

    
        public override string DebugString ( )
        {
            return "Pars:";
        }

     
    }
}