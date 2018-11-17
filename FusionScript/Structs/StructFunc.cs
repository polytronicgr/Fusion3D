﻿using System;

namespace FusionScript.Structs
{
    public class StructFunc : Struct
    {
        public string FuncName = "";
        public StructPars Pars = null;
        public StructCode Code = null;
        public bool Static = false;

        public StructFunc()
        {
            LocalScope = new CodeScope("FuncScope");
        }

        public override dynamic Exec()
        {
            Console.WriteLine("Running Func:" + FuncName);
            Code.Exec();
            return null;
        }

        public override string DebugString ( )
        {
            return "Func:" + FuncName;
        }

     
    }
}