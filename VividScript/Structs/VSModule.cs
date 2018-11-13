using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSModule : VStruct
    {
        public string ModuleName = "";
        public List<VSVar> StaticVars = new List<VSVar>();
        public List<VSVar> Vars = new List<VSVar>();
        public List<VSFunc> StaticFuncs = new List<VSFunc>();
        public List<VSFunc> Methods = new List<VSFunc>();
        public CodeScope StaticScope = new CodeScope("ModuleStatic");
        public CodeScope InstanceScope = new CodeScope("ModuleInstance");
 
        public VSModule ( VTokenStream s ) : base ( s )
        {
 
            Console.WriteLine("VSModule------------------------<<<<<<<<<<<<<<<<<<<<<");
        }

        public override string DebugString ( )
        {
            return "Module:" + ModuleName + " Vars:" + Vars.Count;
        }

        public VSVar FindVar(string name)
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

                                    var text = ConsumeNext();
                                    if(text.Token == Token.Comma)
                                    {
                                        continue;
                                    }
                                    if (text.Token == Token.EndLine)
                                    {
                                
                                     
                                        return;
                                    }
                                    Console.WriteLine("Var:" + text.Text + " static:" + vstatic);
                                    if (vstatic)
                                    {
                                        var svar = new VSVar();
                                        svar.Name = text.Text;
                                        StaticScope.RegisterVar(svar);
                                        StaticVars.Add(svar);
                                    }
                                    else
                                    {
                                        var ivar = new VSVar();
                                        ivar.Name = text.Text;
                                        InstanceScope.RegisterVar(ivar);
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
                                var f = new VSFunc(TokStream);
                                var nt4 = PeekNext();
                                if (fstatic)
                                {
                                    StaticFuncs.Add(f);
                                }
                                else
                                {
                                    Methods.Add(f);
                                }

                                Console.WriteLine("Added func:" + f.FuncName + " static:" + fstatic);
                                var nt2 = PeekNext();
                                
                                break;

                        }

                        break;
                    case TokenClass.Type:

                        Console.WriteLine ( "Parsing Variable definitions." );
                        BackOne ( );
                        VSDefineVars vdef = new VSDefineVars(TokStream);
                        foreach ( VSVar nv in vdef.Vars )
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