using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
using System.IO;
namespace FusionScript
{
    public class Parser
    {

        public StructEntry Entry;

        TokenStream toks = null;

        public CodeToken Peek(int i)
        {
            return toks.Tokes[i];
        }
        public FileStream fs;
        public TextWriter wr;

   

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

        }

        public StructModule ParseModule(ref int i)
        {

            Log("Begun parsing module", i);
            Console.WriteLine("Parsing Module:" + Peek(i + 1).Text);
            var name = Peek(i + 1).Text;
            i = i + 2;

            var mod = new StructModule();

            mod.ModuleName = name;


            for (i = i; i < toks.Len; i++)
            {
                var mtok = toks.Tokes[i];

                switch (mtok.Token)
                {
                    case Token.Func:

                          var func = ParseFunc(ref i);

                        if (func.Static)
                        {

                            mod.StaticFuncs.Add(func);

                        }
                        else
                        {

                            mod.Methods.Add(func);

                        }

                            break;
                    case Token.Var:

                        break;

                }


            }


            return null;
          

        }

        public StructFunc ParseFunc(ref int i)
        {

            bool is_static = false;

            Log("Begun parsing function", i);

            if (toks.Tokes[i].Token != Token.Func)
            {
                Error(i,"Expecting 'func' definition.");
            }

            if (Peek(i + 1).Text == "static")
            {
                is_static = true;
                i++;
            }

            string func_name = Peek(i + 1).Text;

            Console.WriteLine("Module func:" + func_name + " static:" + is_static);

            i+=2;

            StructFunc func = new StructFunc();

            func.Static = is_static;
            func.FuncName = func_name;

            var ftok = toks.Tokes[i];

            if (ftok.Text != "(")
            {
                Error(i, "Expected begining of function definition parameters.");
                //    Console.WriteLine("Error, expected (");
            }


            var pars = new StructCallPars();

            if (toks.Tokes[i + 1].Token != Token.RightPara)
            {
                for (i = i; i < toks.Len; i++)
                {

                    var tok = toks.Tokes[i];

                    switch (tok.Token)
                    {
                        case Token.Id:

                            break;


                    }

                }
            }
            else
            {
                i+=2;
            }


            var code_body = ParseCodeBody(ref i);

            func.Code = code_body;

            return func;

        }

        public void Log(string msg,int i = -1)
        {
            wr.WriteLine(msg);
            if (i != -1)
            {
                string line = GenLine(i);
                wr.WriteLine(line);
               
            }
            wr.WriteLine("");
        }
        public void Error(int i,string err)
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

        private string GenLine(int i)
        {
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

        public StructCode ParseCodeBody(ref int i)
        {

            Log("Begun parsing code body", i);

          

            var code = new StructCode();

            for (i = i; i < toks.Len; i++)
            {

                var st = Predict(i);
                switch(st)
                {
                    case StrandType.Assignment:

                        i = NextToken(i, Token.Id);

                        var assign = ParseAssign(ref i);

                        code.Lines.Add(assign);

                        break;
                }
                

            }


            return code;

        }

        public int NextToken(int i,Token tok)
        {
            int bi = i;
            for (i = i; i < toks.Len; i++)
            {
                if(toks.Tokes[i].Token == tok)
                {
                    return i;
                }
            }
            return bi;
        }

        public StructAssign ParseAssign(ref int i)
        {
            if(toks.Tokes[i].Token != Token.Id)
            {
                Error(i, "Expecting identifier");
            }
            Log("Parsing Assign", i);

            var assign = new StructAssign();

            var var_name = toks.Tokes[i].Text;

            Log("Parsing Var:" + var_name, i);

            AssertTok(i + 1, Token.Equal, "Expecting =");

            i = i + 2;

            var av = new Var();
            av.Name = var_name;

            assign.Vars.Add(av);
            


            StructExpr exp = ParseExp(ref i);

            assign.Expr.Add(exp);


            return assign;

        }

        public StructExpr ParseExp(ref int i)
        {

            Log("Parsing expressionj", i);

            var exp = new StructExpr();

            bool prev_op = false;
            bool first_toke = true;
            bool prev_op_min = false;
            for (i = i;i < toks.Len;i++)
            {

                switch (Get(i).Token)
                {
                    case Token.EndLine:

                        Log("Expr:" + exp.DebugString(), i);

                        return exp;

                        break;
                    case Token.LeftPara:

                        i++;
                        var sub_e = ParseExp(ref i);
                        first_toke = false;

                        break;

                    case Token.Plus:

                        if (prev_op || first_toke)
                        {
                            Error(i, "Expecting value.");
                        }



                        var ep = new StructExpr();
                        ep.Op = OpType.Plus;
                        ep.Type = ExprType.Operator;
                        exp.Expr.Add(ep);
                        prev_op = true;
                        break;


                    case Token.Minus:

                        if (prev_op_min)
                        {

                            Error(i, "too many negations operators.");

                        }

                        if (prev_op)
                        {
                            if (first_toke)
                            {
                                Error(i, "Illegal minus op.");
                            }

                            var nv = new StructExpr();
                            nv.Type = ExprType.Operator;
                            nv.Op = OpType.Minus;
                            exp.Expr.Add(nv);
                            prev_op = true;
                            prev_op_min = true;
                            continue;
                            
                            //Error(i, "Expecting value.");
                        }


                        var em = new StructExpr();
                        em.Type = ExprType.Operator;
                        em.Op = OpType.Minus;
                        exp.Expr.Add(em);
                        prev_op = true;
                        
                        break;
                    case Token.Div:

                        if (prev_op || first_toke)
                        {
                            Error(i, "Expecting value.");
                        }


                        var ed = new StructExpr();
                        ed.Type = ExprType.Operator;
                        ed.Op = OpType.Divide;
                        exp.Expr.Add(ed);

                        prev_op = true;


                        break;
                    case Token.Multi:

                        if (prev_op || first_toke)
                        {
                            Error(i, "Expecting value.");
                        }


                        var eo = new StructExpr();

                        eo.Type = ExprType.Operator;
                        eo.Op = OpType.Times;

                        exp.Expr.Add(eo);


                        prev_op = true;

                        break;

                    case Token.Int:
                    case Token.Float:
                    case Token.Short:
                    case Token.Long:
                    case Token.Double:
                    case Token.String:

                        if (!prev_op && !first_toke) 
                        {
                            Error(i, "Expecting operator.");
                        }
                        prev_op = false;
                        var ev = new StructExpr();
                        ev.intV = int.Parse(Get(i).Text);
                        ev.Type = ExprType.IntValue;
                        exp.Expr.Add(ev);
                        first_toke = false;

                        break;
                    default:
                        Error(i,"Illegal token within expression:");
                        break;
                }

            }

            Log("Expr:" + exp.DebugString(), i);

            return exp;

        }
        
        public CodeToken Get(int i)
            {
                return toks.Tokes[i];
            }
        
        public void AssertTok(int i,Token tok,string err="!")
        {
            if (err == "!")
            {
                err = "Expecting:" + tok.ToString();
            }
            if(toks.Tokes[i].Token != tok)
            {
                Error(i, err);
            }
        }

        public StrandType Predict(int i)
        {
            StrandType ret = StrandType.Unknown;
            for(i = i; i < toks.Len; i++)
            {

                if(toks.Tokes[i].Token==Token.Equal)
                {
                    return StrandType.Assignment;
                }


            }
            return ret;
        }

    }
}
