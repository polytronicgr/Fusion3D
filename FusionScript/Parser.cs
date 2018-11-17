using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
using System.IO;
namespace FusionScript
{
    public partial class Parser
    {

        public StructEntry Entry;

        public FileStream fs;
        public TextWriter wr;
        TokenStream toks = null;

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

            var var_name = toks.Tokes[i].Text;

            Log("Parsing Var:" + var_name, i);

            AssertTok(i + 1, Token.Equal, "Expecting =");

            i = i + 2;

            var av = new Var();
            av.Name = var_name;

            assign.Vars.Add(av);



            StructExpr exp = ParseExp(ref i);

            assign.Expr.Add(exp);

            Log("End of assign:", i);


            return assign;

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

       
        public StructExpr ParseExp(ref int i)
        {

            Log("Parsing expressionj", i);

            var exp = new StructExpr();

            bool prev_op = false;
            bool first_toke = true;
            bool prev_op_min = false;
            for (i = i; i < toks.Len; i++)
            {

                switch (Get(i).Token)
                {
                    case Token.RightPara:
                    case Token.EndLine:
                    case Token.Comma:

                        Log("Expr:" + exp.DebugString(), i);

                        return exp;

                        break;
                    case Token.Id:
                        Log("VarInExpr:" + Get(i).Text, i);
                        var ve = new StructExpr();
                        ve.Type = ExprType.VarValue;
                        ve.VarName = Get(i).Text;
                        exp.Expr.Add(ve);
                        if (!prev_op && !first_toke)
                        {
                            Error(i, "Expecting operator.");
                        }
                        prev_op = false;
                        first_toke = false;

                        break;
                    case Token.LeftPara:

                        i++;
                        var sub_e = ParseExp(ref i);
                        first_toke = false;
                        sub_e.Type = ExprType.SubExpr;
                        exp.Expr.Add(sub_e);

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

                        Log("Times?", i);
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
                    case Token.String:

                        if (!prev_op && !first_toke)
                        {
                            Error(i, "Expecting operator.");
                        }
                        prev_op = false;
                        var es = new StructExpr();
                        es.StringV = Get(i).Text;
                        es.Type = ExprType.StringValue;
                        exp.Expr.Add(es);
                        first_toke = false;

                        break;
                    case Token.Int:
                    case Token.Float:
                    case Token.Short:
                    case Token.Long:
                    case Token.Double:


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
                        Error(i, "Illegal token within expression:");
                        break;
                }

            }

            Log("Expr:" + exp.DebugString(), i);

            return exp;

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
            StrandType ret = StrandType.Unknown;
            for (i = i; i < toks.Len; i++)
            {
                if(toks.Tokes[i].Token == Token.BeginLine)
                {
                    return StrandType.Unknown;
                }
                if (toks.Tokes[i].Token == Token.Equal)
                {
                    return StrandType.Assignment;
                }
                if (toks.Tokes[i].Token == Token.LeftPara)
                {
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
