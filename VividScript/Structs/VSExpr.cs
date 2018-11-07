using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public enum ExprType
    {
        IntValue, FloatValue, StringValue, RefValue, ClassValue, SubExpr, Operator, Unknown, VarValue, BoolValue
    }

    public enum OpType
    {
        Plus, Minus, Divide, Times, Pow2, Percent, EqualTo, LessThan, MoreThan, NotEqual
    }

    public class VSExpr : VStruct
    {
        public List<VSExpr> Expr = new List<VSExpr>();

        public ExprType Type = ExprType.Unknown;
        public OpType Op = OpType.Divide;

        public string StringV="";
        public int intV=0;
        public float floatV=0;
        public bool BoolV = false;
        public string VarName="";

        public override string DebugString ( )
        {
            return "Expr:" + Expr.Count + " Elements";
        }

        public VSExpr ( VTokenStream s, bool no_parse = false ) : base ( s, no_parse )
        {
            Done = true;
        }

        public override dynamic Exec ( )
        {
            return NextE ( 0, null );

            return null;
        }

        public dynamic NextE ( int i, dynamic prev )
        {
            dynamic val="";
            VSExpr e = Expr[i];
            switch ( e.Type )
            {
                case ExprType.SubExpr:
                    val = e.NextE ( 0, null );
                    break;

                case ExprType.Operator:
                    switch ( e.Op )
                    {
                        case OpType.Times:
                            if ( prev is string )
                            {
                                return prev;
                            }
                            return prev * NextE ( i + 1, null );

                        case OpType.Divide:
                            if ( prev is string )
                            {
                                return prev;
                            }
                            return prev / NextE ( i + 1, null );
                            break;

                        case OpType.Plus:
                            return prev + NextE ( i + 1, null );
                            break;

                        case OpType.Minus:
                            return prev - NextE ( i + 1, null );
                    }
                    break;

                case ExprType.VarValue:

                    val = VME.CurrentScope.FindVar ( e.VarName, true ).Value;
                    break;

                case ExprType.IntValue:

                    val = e.intV;
                    break;

                case ExprType.StringValue:

                    val = e.StringV;

                    break;

                case ExprType.BoolValue:
                    val = e.BoolV;
                    break;
            }
            if ( i < Expr.Count - 1 )
            {
                val = NextE ( i + 1, val );
            }
            return val;
        }

        public override void SetupParser ( )
        {
            Console.WriteLine ( "Parsing expression." );
            Parser = ( t ) =>
            {
                Console.WriteLine ( "PE:" + t );

                if ( t.Token == Token.LeftPara )
                {
                    VSExpr sub_e = new VSExpr(TokStream,false);
                    Expr.Add ( sub_e );
                    sub_e.Type = ExprType.SubExpr;
                    return;
                }

                if ( t.Token == Token.RightPara )
                {
                    Done = true;
                    return;
                }
                if ( t.Token == Token.EndLine )
                {
                    Done = true;
                    return;
                }
                switch ( t.Class )
                {
                    case TokenClass.Bool:
                        VSExpr bool_e = new VSExpr ( TokStream, true )
                        {
                            Type = ExprType.BoolValue
                        };
                        Expr.Add ( bool_e );
                        if ( t.Text == "true" )
                        {
                            bool_e.BoolV = true;
                        }
                        break;

                    case TokenClass.Op:
                        VSExpr op_e = new VSExpr(TokStream,true);
                        // Expr.Add ( op_e );
                        switch ( t.Token )
                        {
                            case Token.Plus:
                                op_e.Op = OpType.Plus;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

                            case Token.Multi:
                                op_e.Op = OpType.Times;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

                            case Token.Minus:
                                op_e.Op = OpType.Minus;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

                            case Token.Div:
                                op_e.Op = OpType.Divide;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;
                        }
                        break;

                    case TokenClass.Id:
                        VSExpr ve = new VSExpr ( TokStream, true )
                        {
                            Type = ExprType.VarValue,
                            VarName = t.Text
                        };
                        Expr.Add ( ve );
                        break;

                    case TokenClass.Value:
                        Console.WriteLine ( "Value:" + t.Text );

                        VSExpr exp = new VSExpr(TokStream,true);
                        Expr.Add ( exp );
                        switch ( t.Token )
                        {
                            case Token.Id:
                                exp.Type = ExprType.VarValue;
                                exp.VarName = t.Text;
                                break;

                            case Token.String:
                                exp.Type = ExprType.StringValue;
                                exp.StringV = t.Text;
                                break;

                            case Token.Int:
                                exp.Type = ExprType.IntValue;
                                exp.intV = int.Parse ( t.Text );
                                break;
                        }

                        break;

                    default:
                        Done = true;
                        return;
                        break;
                }
            };
        }
    }
}