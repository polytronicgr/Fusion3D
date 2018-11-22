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
                    case Token.ComputeInput:

                     //   var com_in = ParseComputeInput(ref i);
                      //  Entry.ComInputs.Add(com_in);



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
