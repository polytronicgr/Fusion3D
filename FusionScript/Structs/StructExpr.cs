using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public enum ExprType
    {
        IntValue, FloatValue, StringValue, RefValue, ClassValue, SubExpr, Operator, Unknown, VarValue, BoolValue
    }

    public enum OpType
    {
        Plus, Minus, Divide, Times, Pow2, Percent, EqualTo, LessThan, MoreThan, NotEqual
    }

    public class StructExpr : Struct
    {
        public List<StructExpr> Expr = new List<StructExpr>();

        public ExprType Type = ExprType.Unknown;
        public OpType Op = OpType.Divide;

        public string StringV="";
        public int intV=0;
        public float floatV=0;
        public bool BoolV = false;
        public string VarName="";
        public bool NewClass = false;
        public StructCallPars NewPars;
        public string NewClassType = "";

        public override string DebugString ( )
        {
            return "Expr:" + Expr.Count + " Elements";
        }

        public StructExpr ( TokenStream s, bool no_parse = false ) : base ( s, no_parse )
        {
            Done = true;
        }

        public override dynamic Exec ( )
        {
            if (NewClass)
            {

                var base_class = ManagedHost.Main.FindClass(NewClassType);
                var nvar = new Var();
                nvar.Name = base_class.ModuleName + "_Instnace";
                
                return base_class.CreateInstance();

            }
            return NextE ( 0, null );

            return null;
        }

        public dynamic NextE ( int i, dynamic prev )
        {
            dynamic val="";
            StructExpr e = Expr[i];
            if(e.VarName == ".")
            {
                val = NextE(i + 1, prev);
            }
            if (prev is StructModule && e.VarName != ".") 
            {

                val= prev.FindVar(e.VarName).Value;
            }

            switch ( e.Type )
            {
                case ExprType.SubExpr:
                    val = e.NextE ( 0, null );
                    break;

                case ExprType.Operator:
                    switch ( e.Op )
                    {
                        case OpType.LessThan:

                            return prev < NextE ( i + 1, null );

                        case OpType.MoreThan:

                            return prev > NextE ( i + 1, null );

                        case OpType.EqualTo:

                            return prev == NextE ( i + 1, null );

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

                    if(e.VarName == ".")
                    {
                        return NextE(i + 1, prev);
                    }

                    if (prev is StructModule)
                    {
                        val = prev.FindVar (e.VarName).Value;
                    }
                    else
                    {
                        val = ManagedHost.CurrentScope.FindVar(e.VarName, true).Value;
                    }
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
            if(i<Expr.Count-1)
            {
                return  NextE(i + 1, val);
            }
            return val;
        }

        public override void SetupParser ( )
        {
     
            Parser = ( t ) =>
            {
 

                if (t.Text == "=")
                {
                    return;
                }

                if ( t.Token == Token.LeftPara )
                {
                    StructExpr sub_e = new StructExpr(TokStream,false);
                    Expr.Add ( sub_e );
                    sub_e.Type = ExprType.SubExpr;
                    return;
                }
                if(t.Text == ".")
                {
                    Done = true;
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
                    case TokenClass.New:

                        NewClass = true;

                        var new_name = ConsumeNext();

                       // Console.WriteLine("NewClass:" + new_name.Text);
                        NewPars = new StructCallPars(TokStream);
                      //  Console.WriteLine("Pars:" + NewPars.Pars.Count);
                        NewClassType = new_name.Text;

                        break;
                    case TokenClass.Bool:
                        StructExpr bool_e = new StructExpr ( TokStream, true )
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
                        StructExpr op_e = new StructExpr(TokStream,true);
                        // Expr.Add ( op_e );
                        switch ( t.Token )
                        {
                            case Token.Greater:
                                op_e.Op = OpType.MoreThan;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

                            case Token.Lesser:
                                op_e.Op = OpType.LessThan;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

                            case Token.Equal:
                                op_e.Op = OpType.EqualTo;
                                op_e.Type = ExprType.Operator;
                                Expr.Add ( op_e );
                                break;

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
                        StructExpr ve = new StructExpr ( TokStream, true )
                        {
                            Type = ExprType.VarValue,
                            VarName = t.Text
                        };
                        Expr.Add ( ve );
                        break;

                    case TokenClass.Value:
      
                        StructExpr exp = new StructExpr(TokStream,true);
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