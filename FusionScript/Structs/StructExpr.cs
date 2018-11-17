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

     

        public override dynamic Exec ( )
        {
            if (NewClass)
            {

                var base_class = ManagedHost.Main.FindClass(NewClassType);
                var nvar = new Var();
                nvar.Name = base_class.ModuleName + "_Instnace";
                
                var bc= base_class.CreateInstance();

                ManagedHost.Main.ExecuteMethod(bc, base_class.ModuleName, null);

                return bc;

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

    
    }
}