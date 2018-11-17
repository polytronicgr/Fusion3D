using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructModule : Struct
    {
        public string ModuleName = "";
        public List<Var> StaticVars = new List<Var>();
        public List<Var> Vars = new List<Var>();
        public List<StructFunc> StaticFuncs = new List<StructFunc>();
        public List<StructFunc> Methods = new List<StructFunc>();
        public CodeScope StaticScope = new CodeScope("ModuleStatic");
        public CodeScope InstanceScope = new CodeScope("ModuleInstance");
 
     
        public StructModule()
        {

        }

        public override string DebugString ( )
        {
            return "Module:" + ModuleName + " Vars:" + Vars.Count;
        }

        public Var FindVar(string name)
        {
            foreach(var cv in Vars)
            {
                if (cv.Name == name)
                {
                    return cv;
                }
            }
            var rv = StaticScope.FindVar(name, true);
            return rv;
            return null;
        }

        
        public StructModule CreateInstance()
        {

            var ret = new StructModule();
            ret.ModuleName = ModuleName;
            ret.Methods = Methods;
            ret.StaticFuncs = StaticFuncs;
            ret.StaticVars = StaticVars;
            foreach(var v in Vars)
            {

                var nv = new Var();
                nv.Name = v.Name;
                nv.Value = v.Init.Exec();

                ret.Vars.Add(nv);
                ret.InstanceScope.RegisterVar(nv);

            }

            return ret;
        }
       
    }
}