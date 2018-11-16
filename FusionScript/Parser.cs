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

          

            toks = stream;

            Log("Begun parsing source.", 0);

            for (int i = 0; i < stream.Len; i++)
            {
                var tok = stream.Tokes[i];

                switch (tok.Token)
                {
                    case Token.Module:

                        var mod = ParseModule(i);

                        break;
                        
                    default:
                      //  Error(i, "Expected module/func or similar construct definition");
                        break;
                }


            }

            wr.Flush();
            wr.Close();

        }

        public StructModule ParseModule(int i)
        {

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

                          var func = ParseModuleFunc(i);

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

        public StructFunc ParseModuleFunc(int i)
        {

            bool is_static = false;

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

            var code_body = ParseCodeBody(i);

            func.Code = code_body;

            return func;

        }

        public void Log(string msg,int i = -1)
        {
            wr.WriteLine("Log:" + msg);
            if (i != -1)
            {
                string line = GenLine(i);
                wr.WriteLine(line);
               
            }
        }
        public void Error(int i,string err)
        {
            string line = GenLine(i);

            Console.WriteLine("Err:" + err);
            Console.WriteLine(line);




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
                    line = line + "]";
                }
                line = line + " ";

            }

            return line;
        }

        public StructCode ParseCodeBody(int i)
        {

            var ftok = toks.Tokes[i];

            if(ftok.Text != "(")
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

            }

            var code = new StructCode();




            return code;

        }



    }
}
