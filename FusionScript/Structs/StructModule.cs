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

        public bool IsFunc(string name)
        {

            foreach(var f in Methods)
            {
                if(f.FuncName == name)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasFunc(string name)
        {
            foreach(var m in Methods)
            {
                if(m.FuncName == name)
                {
                    return true;
                }
            }
            return false;

        }
        public dynamic ExecFunc(string name,dynamic[] pars)
        {
            foreach(var m in Methods)
            {
                if(m.FuncName == name)
                {

                    return ManagedHost.Main.ExecuteMethod(this, name, pars);


                }
            }
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
                try
                {
                    nv.Value = v.Init.Exec();
                }
                catch { 
                    nv.Value = 0;
                }
                ret.Vars.Add(nv);
                ret.InstanceScope.RegisterVar(nv);

            }

            return ret;
        }
       
    }
}