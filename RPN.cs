using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

[assembly: System.Security.AllowPartiallyTrustedCallers]

namespace ReversePolishNotation
{
    #region Delegates

    public delegate void RPNFunctionProcess(RPNFunctionEventArgs args);

    public class RPNFunctionEventArgs
    {
        public RPNFunctionEventArgs()
        {
            Parameters = new Stack<Token>();
        }

        public string FunctionName;
        public Stack<Token> Parameters;
        public Token Result;
    }

    #endregion

    /// <summary>
    /// Reverse Polish Notation with custom function support.
    /// Based on wikipedia shufling yard algorythm description
    /// Pakowski 2012.         
    /// </summary>
    public class RPN
    {
        private Stack<Token> Stack = new Stack<Token>();
        private List<string> Operators = new List<string>(new string[] { "+", "-", "*", "/", ">", "<", ">=", "<=", "!=", "<>", "==", "=", "||", "&&" });
        private string TokenStrings = "(\"[^\"]+\")|(\\|\\|)|(\\&\\&)|(>\\=)|(<\\=)|(\\!\\=)|(<>)|(\\=\\=)|(\\+)|(,)|(-)|(\\*)|(/)|(>)|(<)|(\\=)|(\\()|(\\))|(\\=)";

        public List<Token> Output = new List<Token>();
        public RPNFunctionProcess ProcessFunction;

        /// <summary>
        /// Procedure converts infix notation to reverse polish notation
        /// </summary>
        /// <param name="infix"></param>
        /// <returns></returns>
        public List<Token> ToRPN(string infix)
        {
            List<Token> tokens = new List<Token>();
            String tmp = String.Empty;
            Token token = null;
            Stack.Clear();

            StringTokenizer st = new StringTokenizer(infix, TokenStrings, Operators.ToArray());
            tokens = st.Tokenize();

            for (int i = 0; i < tokens.Count; i++)
            {
                token = tokens[i];

                switch (token.TokenType)
                {
                    case TokenType.Number:
                    case TokenType.Variable:
                    case TokenType.String:
                    case TokenType.Date:
                        Output.Add(token);
                        break;

                    case TokenType.Operator:
                        while (Stack.Count > 0 && Priority(Stack.Peek().Content) >= Priority(token.Content))
                            Output.Add(Stack.Pop());
                        Stack.Push(token);
                        break;

                    case TokenType.LeftBracket:
                        Stack.Push(token);
                        break;

                    case TokenType.Function:
                        Stack.Push(token);
                        break;

                    case TokenType.RightBracket:

                        while (Stack.Count > 0 && Stack.Peek().TokenType != TokenType.LeftBracket)
                            Output.Add(Stack.Pop());

                        if (Stack.Count > 0 && Stack.Peek().TokenType == TokenType.LeftBracket)
                            Stack.Pop();

                        if (Stack.Count > 0 && Stack.Peek().TokenType == TokenType.Function)
                            Output.Add(Stack.Pop());

                        break;

                    default:
                        break;
                }
            }

            while (Stack.Count > 0)
                Output.Add(Stack.Pop());

#if DEBUG
            Console.WriteLine("RPN:");
            for (int i = 0; i < Output.Count; i++)
                Console.Write("{0};", Output[i].Content);
            Console.WriteLine();
#endif

            return Output;
        }

        /// <summary>
        /// Procedure evaluate infix expression
        /// </summary>
        /// <param name="infix"></param>
        /// <returns>Token which can contains number, variable or string</returns>
        public Token Eval(string infix)
        {
            Output.Clear();
            ToRPN(infix);
            Stack.Clear();

            for (int i = 0; i < Output.Count; i++)
            {
                if (Output[i].TokenType == TokenType.Operator)
                {
                    Token right = Stack.Pop();
                    Token left = Stack.Pop();

                    try
                    {
                        Stack.Push(TokenOperatorFacade.ProcessOperator(left, right, Output[i]));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            String.Format("Error processing operator: {0}. Right argument:{1} [{2}]. Left argument:{3} [{4}]  ", Output[i].Content, right.Content, right.TokenType, left.Content, left.TokenType),
                            ex);
                    }
                }
                else if (Output[i].TokenType == TokenType.Function)
                {
                    try
                    {
                        RPNFunctionEventArgs args = new RPNFunctionEventArgs();
                        args.FunctionName = Output[i].Content;
                        args.Parameters = Stack;
                        ProcessFunction(args);
                        Stack.Push(args.Result);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error processing function: " + Output[i].Content, ex);
                    }
                }
                else
                {
                    Stack.Push(Output[i]);
                }
            }

            return Stack.Peek();
        }

        /// <summary>
        /// Returns priority of operator
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        private static int Priority(string oper)
        {
            oper = oper.Trim();

            if (oper.Equals("^"))
                return 4;
            if (oper.Equals("*") || oper.Equals("/"))
                return 3;
            if (oper.Equals("+") || oper.Equals("-"))
                return 2;
            else
                return 1;
        }
    }
}
