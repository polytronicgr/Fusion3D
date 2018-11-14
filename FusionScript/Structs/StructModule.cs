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
 
        public StructModule ( TokenStream s ) : base ( s )
        {
 

        }

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
        public override void SetupParser ( )
        {
            PreParser = ( t ) =>
            {
                ModuleName = t.Text;
                StaticScope = new CodeScope("ModuleStatic");
                StaticScope.ScopeID = "Module:" + t.Text + " Static";
            };
            Parser = ( t ) =>
            {
                if(t.Token == Token.Module)
                {
                    BackOne();
                    Done = true;
                    return;
                }
                if(Peek(0).Token == Token.End)
                {
                    BackOne();
                    Done = true;
                    return;
                }
                if ( t.Token == Token.End )
                {
                    Done = true;
                    return;
                }
                switch ( t.Class )
                {
                    case TokenClass.Define:

                        switch (t.Token)
                        {
                            case Token.Var:

                                var next = PeekNext();

                                bool vstatic = false;

                                if (next.Text=="static")
                                {
                                    vstatic = true;
                                    ConsumeNext();
                                }

                                while (true)
                                {

                                    var cur = Peek(TokStream.Pos);
                                    var text = ConsumeNext();
                                    if(text.Token == Token.Comma)
                                    {
                                        continue;
                                    }
                                    if (text.Token == Token.EndLine)
                                    {
                                
                                     
                                        return;
                                    }
                                    Var newv = null;
                                 
                                    if (vstatic)
                                    {
                                        var svar = new Var();
                                        svar.Name = text.Text;
                                        StaticScope.RegisterVar(svar);
                                        StaticVars.Add(svar);
                                        newv = svar;
                                    }
                                    else
                                    {
                                        var ivar = new Var();
                                        ivar.Name = text.Text;
                                        Vars.Add(ivar);
                                        newv = ivar;
                                        //  InstanceScope.RegisterVar(ivar);
                                    }
                                    var nt6 = PeekNext();
                                    if(nt6.Text == "=")
                                    {
                                        var ntok = PeekNext();
                                      
                                        newv.Init = new StructExpr(TokStream);
                                        ntok = PeekNext();
                                        ntok = Peek(0);
                                        if(ntok.Text == ";")
                                        {
                                            return;
                                        }
                                    }

                                }

                                Console.WriteLine("@");

                                break;
                            case Token.Func:

                                bool fstatic = false;
                                var nt = PeekNext();

                                if(nt.Text == "static")
                                {
                                    fstatic = true;
                                    ConsumeNext();
                                }
                                var nt3 = PeekNext();
                                var f = new StructFunc(TokStream);
                                var nt4 = PeekNext();
                                if (fstatic)
                                {
                                    StaticFuncs.Add(f);
                                }
                                else
                                {
                                    Methods.Add(f);
                                }

                           
                                var nt2 = PeekNext();
                                Console.WriteLine("Method:" + f.FuncName);
                                
                                break;

                        }

                        break;
                    case TokenClass.Type:

                    
                        BackOne ( );
                        StructDefineVars vdef = new StructDefineVars(TokStream);
                        foreach ( Var nv in vdef.Vars )
                        {
                            Vars.Add ( nv );
                        }
                        //Structs.Add(vdef);

                        break;
                }
            };
        }
    }
}