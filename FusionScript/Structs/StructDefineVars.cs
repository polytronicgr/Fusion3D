using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructDefineVars : Struct
    {
        public VarType Type = VarType.Bool;
        public List<Var> Vars = new List<Var>();

   
    }
}