using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
using FusionScript.Structs.Compute;
using FusionScript.Structs.Compute.ComputeCodeTypes;
using System.IO;


namespace FusionScript
{
    public partial class Parser
    {

        public StructEntry Entry;

        public FileStream fs;
        public TextWriter wr;
        TokenStream toks = null;
        List<StructComputeStruct> ComStructs = new List<StructComputeStruct>();

   
        public StructComputeStruct ParseComputeStruct(ref int i)
        {
            var rs = new StructComputeStruct();

            i++;

            rs.StructName = Get(i).Text;

            i++;

            if(Get(i).Text=="=")
            {

                i++;
                var tt = Get(i).Text;
                bool pointer = false;
                if (Get(i + 1).Text == "*")
                {
                    pointer = true;
                }
                var var = new ComputeVar();
                var.Name = rs.StructName;
                var.Pointer = pointer;

                rs.LinearData = true;

                switch (tt)
                {
                    case "byte":
                        var.Type = ComputeVarType.Byte;
                        break;
                    case "float":
                        var.Type = ComputeVarType.Float;
                        break;
                    case "vec3":
                        var.Type = ComputeVarType.Vec3;
                        break;
                    case "vec4":
                        var.Type = ComputeVarType.Vec4;
                        break;
                }
                rs.Vars.Add(var);
                i++;
                return rs;
            }

            for (i = i; i < toks.Len; i++)
            {

                var tok = Get(i);

                switch (tok.Token)
                {
                    case Token.End:
                        return rs;
                        break;
                    case Token.Byte:

                        var byte1 = new ComputeVar();
                        byte1.Type = ComputeVarType.Byte;
                   
                        if(Get(i+1).Text == "*")
                        {
                            byte1.Pointer = true;
                            i++;
                        }
                        byte1.Name = Get(i + 1).Text;
                        i += 2;
                        rs.Vars.Add(byte1);
                        break;
                    case Token.Int:

                        var int1 = new ComputeVar();
                        int1.Type = ComputeVarType.Int;
                 
                        if (Get(i + 1).Text == "*")
                        {
                            int1.Pointer = true;
                            i++;
                        }
                        int1.Name = Get(i + 1).Text;
                        i += 2;
                        rs.Vars.Add(int1);

                        break;
                    case Token.Vec3:

                        var vec3 = new ComputeVar();
                        vec3.Type = ComputeVarType.Vec3;
                  
                        if(Get(i+1).Text == "*")
                        {
                            vec3.Pointer = true;
                            i++;
                        }
                        vec3.Name = Get(i + 1).Text;
                        i += 2;
                        Console.WriteLine("CV:" + vec3.Name + " Type:" + vec3.Type.ToString());

                        rs.Vars.Add(vec3);

                        break;
                }

            }

            return rs;

        }
        public enum ComputeStrandType
        {
            Assign,Statement,Return,Unknown,For,While,Switch,Do,Loop
        }
        public ComputeStrandType PredictCompute(int i)
        {

            for (i = i; i < toks.Len; i++)
            {

                var tok = Get(i);

                switch (tok.Text)
                {
                    case "int":
                    case "float":
                    case "vec3":
                    case "vec4":
                    case "vec2":
                    case "matrix3":
                    case "matrix4":
                        return ComputeStrandType.Assign;
                        break;
                }

                if(tok.Text == "for")
                {
                    return ComputeStrandType.For;
                }
                if(tok.Text == "while")
                {
                    return ComputeStrandType.While;
                }

                if(tok.Text == "switch")
                {
                    return ComputeStrandType.Switch;
                }

                if(tok.Text=="=")
                {
                    return ComputeStrandType.Assign;
                }
                if(tok.Text =="(")
                {
                    return ComputeStrandType.Statement;
                }
                if(tok.Text == "return")
                {
                    return ComputeStrandType.Return;
                }


            }
            return ComputeStrandType.Unknown;

        }

        public ComputeCodeExpr ParseComputeExpr(ref int i)
        {
            var rs = new ComputeCodeExpr();

            int pc = 0;

            for (i = i; i < toks.Len; i++)
            {

                var t = Get(i);
                if (t.Text == ";")
                {
                    return rs;
                }
                if(t.Text == "(")
                {
                    pc++;
                }
                if(t.Text == ")")
                {
                    if(pc==0)
                    {
                        return rs;
                    }
                    else
                    {
                        pc--;
                    }
                }
                rs.Seq.Add(t.Text);

            }
            return rs;



        }
        public StructComputeCode ParseComputeCode(ref int i)
        {
            var rs = new StructComputeCode();
            bool fin = false;
            for (i = i; i < toks.Len; i++)
            {
                bool done = false;
            fin = false;
                switch (Get(i).Text)
                {
                    case "end":
                        done = true;
                        fin = true;
                        break;
                    case "int":
                    case "for":
                    case "while":
                    case "if":
                    case "vec3":
                    case "return":
                        done = true;

                        break;
                    default:
                        if (Get(i).Token == Token.Id)
                        {
                            done = true;
                        }
                        break;
                }
                if (done) break;
            }

            if (fin) return rs;


            for (i = i; i < toks.Len; i++)
            {

                var tok = Get(i);

                if(tok.Text == "end")
                {
                    break;
                }
                if(tok.Token == Token.BeginLine)
                {
                    continue;
                }

                var cp = PredictCompute(i);
                switch (cp)
                {
                    case ComputeStrandType.Statement:

                        Console.WriteLine("Parsing statement");

                        break;
                    case ComputeStrandType.For:

                        var c_for = new ComputeCodeFor();

                        var tok2 = Get(i);
                        Console.WriteLine("Tok2:" + tok2);
                        i += 2;
                        var for_as = ParseComputeAssign(ref i);

                        c_for.InitAssign = for_as;


                        tok2 = Get(i);

                        if (tok2.Text == ";") 
                        {
                            i++;
                        }

                        var for_exp = ParseComputeExpr(ref i);


                        c_for.Condition = for_exp;

                        tok2 = Get(i);

                        if (tok2.Text == ";")
                        {
                            i++;
                        }

                        c_for.Inc = ParseComputeAssign(ref i);


                        rs.Lines.Add(c_for);

                        i++;

                        tok2 = Get(i);

                        c_for.Code = ParseComputeCode(ref i);



                        break;
                    case ComputeStrandType.Assign:

                        var cass = ParseComputeAssign(ref i);
                        rs.Lines.Add(cass);

                        break;
                }

                Console.WriteLine("ComCodePreDict:" + cp.ToString());

            }


                

            


            //Console.WriteLine("CodeBegin:" + tok);


            return rs;

        }

        private ComputeCodeAssign ParseComputeAssign(ref int i)
        {
            var tok = Get(i);
            var al = new ComputeCodeAssign();
       
            switch (tok.Text)
            {
                case "int":
                case "vec3":
                case "vec4":
                case "vec2":
                case "float":
                case "matrix3":
                case "matrix4":
                case "bool":

                    switch (tok.Text)
                    {
                        case "int":
                            al.Type = ComputeVarType.Int;
                            break;
                        case "vec3":
                            al.Type = ComputeVarType.Vec3;
                            break;
                    }

                    al.Init = true;
                    i++;

                    break;
            }

            var a_name = Get(i).Text;

            i++;

            switch (Get(i).Text)
            {
                case ";":
                    Console.WriteLine("No init");
                    break;
                case "=":
                    i++;
                    var init_exp = ParseComputeExpr(ref i);
                    al.Value = init_exp;
                    break;
            }

            Console.WriteLine("Name:" + a_name);

            al.VarName = a_name;
            return al;
        }

        public StructCompute ParseCompute(ref int i)
        {

            var rs = new StructCompute();

            i++;

            rs.ComputeName = Get(i).Text;

            Console.WriteLine("Compute Name:" + rs.ComputeName);

            i++;

            for (i = i; i < toks.Len; i++)
            {

                var tok = Get(i);

                if(tok.Token == Token.Func)
                {

                    var rt = Get(i + 1);

                    if(rt.Text==">")
                    {

                        var mf = new StructComputeFunc();
                        mf.FuncName = rs.ComputeName + "_main";
                        i++;
                        mf.Code = ParseComputeCode(ref i);
                        return rs;

                    }

                    var fname = Get(i + 2).Text;

                    i += 3;

                    Console.WriteLine("ComputeFunc:"+fname+" ReturnType:"+rt.Text);

                    var com_f = new StructComputeFunc();
                    com_f.FuncName = fname;
                    rs.Funcs.Add(com_f);
                    switch (rt.Text)
                    {
                        case "void":
                            com_f.ReturnType = ComputeVarType.Void;
                            break;
                        case "vec3":
                            com_f.ReturnType = ComputeVarType.Vec3;
                            break;
                    }

                    if(Get(i).Token == Token.LeftPara)
                    {
                        Console.WriteLine("Parsing inputs");
                        i++;
                        for (i = i; i < toks.Len; i++)
                        {

                            var var_t = Get(i);
                            if(var_t.Token == Token.RightPara)
                            {
                                break;
                            }

                            var cv = new ComputeVar();


                            switch (var_t.Text)
                            {
                                case "vec3":
                                    cv.Type = ComputeVarType.Vec3;
                                    var v_name2 = Get(i + 1).Text;
                                    if (v_name2 == "*")
                                    {
                                        cv.Pointer = true;
                                        v_name2 = Get(i + 2).Text;
                                        i++;
                                    }


                                    i++;
                                    cv.Name = v_name2;



                                    com_f.InVars.Add(cv);

                                    break;
                                case "int":

                                    cv.Type = ComputeVarType.Int;
                                   
                                    var v_name = Get(i + 1).Text;
                                    if(v_name=="*")
                                    {
                                        cv.Pointer = true;
                                        v_name = Get(i + 2).Text;
                                        i++;
                                    }

                                    i++;
                                    Console.WriteLine("int:" + v_name);
                                    cv.Name = v_name;

                                    com_f.InVars.Add(cv);

                                    break;
                                default:

                                    foreach(var str in rs.Unique)
                                    {
                                        if(str.StructName == var_t.Text)
                                        {

                                            cv.Type = ComputeVarType.Struct;
                                            cv.StructName = str.StructName;
                                            var v_name3= Get(i + 1).Text;
                                            if (v_name3 == "*")
                                            {
                                                cv.Pointer = true;
                                                v_name3 = Get(i + 2).Text;
                                                i++;
                                            }

                                            i++;
                                            cv.Name = v_name3;
                                            com_f.InVars.Add(cv);

                                        }
                                    }

                                    break;
                            }


                        }

                        var com_c = ParseComputeCode(ref i);
                        com_f.Code = com_c;

                    }

                    if(tok.Token == Token.End)
                    {
                        tok.Token = Token.BeginLine;
                        break;
                    }

                }

                if(tok.Token == Token.End)
                {
                    return rs;
                }

                if (tok.Token == Token.Id)
                {

                    Console.WriteLine("Struct:" + tok.Text);

                    StructComputeStruct str = null;

                    foreach (var sr in ComStructs)
                    {
                        if (sr.StructName == tok.Text)
                        {
                            str = sr;
                        }
                    }

                    str = str.Copy();


                    if (str == null)
                    {
                        Error(i, "Struct:" + tok.Text + " not found.");
                    }

                    var local_name = Get(i + 1).Text;
                    i += 2;
                    str.LocalName = local_name;

                    bool is_in = false;

                    switch (Get(i).Text)
                    {
                        case "in":

                            is_in = true;
                            rs.Inputs.Add(str);

                            break;
                        case "out":

                            rs.Outputs.Add(str);

                            break;
                    }
                    if(Get(i+1).Text == "one")
                    {
                        str.One = true;
                        i++;
                    }

                    bool found = false;
                    foreach(var ts in rs.Unique)
                    {
                        if(ts.StructName == str.StructName)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        rs.Unique.Add(str);
                    }

                    Console.WriteLine("Struct:" + str.StructName + " LocalName:" + str.LocalName + " In:" + is_in);

                }



            }
            return rs;

        }

        public Parser(TokenStream stream)
        {

            wr = File.CreateText("parserLog.txt");

            Entry = new StructEntry();

            toks = stream;

            Log("Begun parsing source.", 1);

            for (int i = 0; i < stream.Len; i++)
            {
                var tok = stream.Tokes[i];

                switch (tok.Token)
                {

                    case Token.Compute:

                        var com = ParseCompute(ref i);
                        Entry.Coms.Add(com);

                        break;
                    case Token.ComputeStruct:

                        

                           var com_str = ParseComputeStruct(ref i);
                        //  Entry.ComInputs.Add(com_in);
                        Console.WriteLine("StructCom");
                        Entry.ComStructs.Add(com_str);
                        ComStructs.Add(com_str);


                    break;
                    case Token.Func:

                        var func = ParseFunc(ref i);

                        Entry.SystemFuncs.Add(func);

                        break;
                    case Token.Module:

                        var mod = ParseModule(ref i);

                        Entry.Modules.Add(mod);

                        break;

                    default:
                        //  Error(i, "Expected module/func or similar construct definition");
                        break;
                }


            }

            wr.Flush();
            wr.Close();

            foreach(var com in Entry.Coms)
            {
                com.GenCode();
            }

        }

        public void AssertTok(int i, Token tok, string err = "!")
        {
            if (err == "!")
            {
                err = "Expecting:" + tok.ToString();
            }
            if (toks.Tokes[i].Token != tok)
            {
                Error(i, err);
            }
        }

        public void Error(int i, string err)
        {
            string line = GenLine(i);

            Console.WriteLine("Err:" + err);
            Console.WriteLine(line);

            wr.Flush();
            wr.Close();

            while (true)
            {

            }


        }

        public CodeToken Get(int i)
        {
            return toks.Tokes[i];
        }

        public void Log(string msg, int i = -1)
        {
            wr.WriteLine(msg);
            if (i != -1)
            {
                string line = GenLine(i);
                wr.WriteLine(line);

            }
            wr.WriteLine("");
        }

        public int NextToken(int i, Token tok)
        {
            int bi = i;
            for (i = i; i < toks.Len; i++)
            {
                if (toks.Tokes[i].Token == tok)
                {
                    return i;
                }
            }
            return bi;
        }

        public StructAssign ParseAssign(ref int i)
        {
            if (toks.Tokes[i].Token != Token.Id)
            {
                Error(i, "Expecting identifier");
            }
            Log("Parsing Assign", i);

            var assign = new StructAssign();

            while (true)
            {
                var var_name = toks.Tokes[i].Text;
                if(var_name==".")
                {
                    i++;
                    continue;
                }
                if (var_name == "=")
                {
                    i--;
                    break;
                }
                else
                {
                    i++;
                    var nv = new Var();
                    nv.Name = var_name;
                    assign.Vars.Add(nv);
                }

            }
     
            AssertTok(i + 1, Token.Equal, "Expecting =");

            i = i + 2;

            //            assign.Vars.Add(av);



            StructExpr exp = ParseExp(ref i);

            assign.Expr.Add(exp);

            Log("End of assign:", i);


            return assign;

        }

        public StructParameters ParseParameters(ref int i)
        {

            var sp = new StructParameters();

            Console.WriteLine("PP:" + Get(i).Text);
            Log("Checking parameters.", i);
            AssertTok(i, Token.LeftPara);

            i++;
            for (i = i; i < toks.Len; i++)
            {
                switch (Get(i).Token)
                {
                    case Token.Id:

                        var vv = new Var();
                        vv.Name = Get(i).Text;
                        sp.Pars.Add(vv);

                        break;
                    case Token.Comma:

                        break;
                    case Token.RightPara:
                        return sp;
                        break;

                }

            }

            return sp;

        }

        public StructCallPars ParseCallPars(ref int i)
        {

            var cp = new StructCallPars();
            AssertTok(i, Token.LeftPara);
            if (Get(i + 1).Token == Token.RightPara)
            {
                return cp;
            }

            i++;

            for (i = i; i < toks.Len; i++)
            {

                if (Get(i).Token == Token.RightPara)
                {
                    return cp;
                }
                var exp = ParseExp(ref i);
                i--;
                cp.Pars.Add(exp);

            }
            return cp;
        }

       
      
        public StructFlatCall ParseFlatStatement(ref int i)
        {

            Log("BeginFS:", i);
            var flat = new StructFlatCall();

            var name = Get(i).Text;

            flat.FuncName = name;
            Log("Func:" + name);

            i++;
            var callpars = ParseCallPars(ref i);
            flat.CallPars = callpars;

            return flat;

        }

   

        public CodeToken Peek(int i)
        {
            return toks.Tokes[i];
        }
        public StrandType Predict(int i)
        {
            bool access = false;
            StrandType ret = StrandType.Unknown;
            for (i = i; i < toks.Len; i++)
            {
                if(toks.Tokes[i].Token == Token.Return)
                {
                    return StrandType.Return;
                }
                if(toks.Tokes[i].Token == Token.For)
                {
                    return StrandType.For;
                }
                if(toks.Tokes[i].Token == Token.If)
                {
                    return StrandType.If;
                }
                if(toks.Tokes[i].Token == Token.While)
                {
                    return StrandType.While;
                }
                if(toks.Tokes[i].Token == Token.BeginLine)
                {
                    return StrandType.Unknown;
                }
                if (toks.Tokes[i].Token == Token.Equal)
                {
                    return StrandType.Assignment;
                }
                if(toks.Tokes[i].Text == ".")
                {
                    access = true;
                }
                if (toks.Tokes[i].Token == Token.LeftPara)
                {
                    if (access)
                    {
                        return StrandType.ClassStatement;
                    }
                    return StrandType.FlatStatement;
                }


            }
            return ret;
        }

        private string GenLine(int i)
        {
            if (i >= toks.Tokes.Count - 1) return "EOF";
            int begin = 0;
            int end = toks.Len;
            for (int ci = i; ci >= 0; ci--)
            {
                if (toks.Tokes[ci].Token == Token.BeginLine)
                {
                    begin = ci;
                    break;
                }
            }

            for (int ci = i; ci < toks.Len; ci++)
            {
                if (toks.Tokes[ci].Token == Token.BeginLine)
                {
                    end = ci;
                    break;
                }
            }
            var line = "";
            for (int ic = begin; ic <= end; ic++)

            {
                if (ic == i)
                {
                    line = line + "[";
                }
                line = line + toks.Tokes[ic].Text;
                if (ic == i)
                {
                    line = line + "](" + toks.Tokes[ic].Token + ")(" + toks.Tokes[ic].Class + ")";
                }
                line = line + " ";

            }

            return line;
        }
    }
}
