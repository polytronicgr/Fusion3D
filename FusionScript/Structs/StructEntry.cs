﻿using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructEntry : Struct
    {
        

        public List<StructModule> Modules = new List<StructModule>();
        public List<StructFunc> SystemFuncs = new List<StructFunc>();

        public override string DebugString ( )
        {
            return "EntryPoint: Modules:" + Modules.Count + " SysFuncs:" + SystemFuncs.Count;
        }

        public StructFunc FindSystemFunc ( string name )
        {
            foreach ( StructFunc func in SystemFuncs )
            {
                if ( func.FuncName == name )
                {
                    return func;
                }
            }
            return null;
        }

  
    }
}