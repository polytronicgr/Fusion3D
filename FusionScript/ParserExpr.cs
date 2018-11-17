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
                    case Token.New:
                        Console.WriteLine("New");
                        var newe = new StructExpr();
                        newe.NewClass = true;
                        newe.Type = ExprType.NewClass;
                        newe.NewClassType = Get(i + 1).Text;
                        Log("New Class:" + newe.NewClassType);
                        i = NextToken(i, Token.RightPara);
                        exp.Expr.Add(newe);
                        return exp;
                        break;
                            break;
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


    }
}
