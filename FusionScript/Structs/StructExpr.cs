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
            //Val.Clear();
            Stack.Clear();
            Output.Clear();
            ToRPN(Expr);




            Stack<StructExpr> Exp = new Stack<StructExpr>();
            foreach (var v in Expr)
            {
                Exp.Push(v);
            }
            return (dynamic)ParseInt(Exp);
            return null;
        }

        public int CalcInt()
        {
            Stack.Clear();
            for (int i = 0; i < Output.Count; i++)
            {
                if (Output[i].Type == ExprType.Operator)
                {
                    var left = Stack.Pop();
                    var right = Stack.Pop();
                    switch(Output[i].Op)
                    {
                        case OpType.Plus:
                            var ne1 = new StructExpr();
                            ne1.intV = left.intV + right.intV; 
                                Stack.Push(ne1);
                        break;
                    }
                    //switch(Output[i].)

                }
                else
                {
                    Stack.Push(Output[i]);
                }

            }

            return 0;
        }
        List<StructExpr> Output = new List<StructExpr>();
        Stack<StructExpr> Stack = new Stack<StructExpr>();
    
        public void ToRPNInt(List<StructExpr> exp)
        {

            Output.Clear();

            for(int i = 0; i < exp.Count; i++)
            {
                switch (exp[i].Type)
                {
                    case ExprType.IntValue:
                        Output.Add(exp[i]);
                        break;
                    case ExprType.Operator:
                        while (Stack.Count > 0 && Priority(Stack.Peek()) >= Priority(exp[i]))
                            Output.Add(Stack.Pop());
                        Stack.Push(exp[i]);
                        //{
                        //   o
                        //}
                        break;
                    
                }

            }
            while (Stack.Count > 0)
                Output.Add(Stack.Pop());
        }

      

        public int ParseInt(Stack<StructExpr> vals)
        {

            StructExpr val = vals.Pop();
            int x=0, y=0;
            if ((val.Type == ExprType.Operator))
            {

                y = ParseInt(vals);
                x = ParseInt(vals);
                if (val.Op == OpType.Plus) x += y;
                else if (val.Op == OpType.Minus) x -= y;
                else if (val.Op == OpType.Times) x *= y;
                else if (val.Op == OpType.Divide) x /= y;
                else throw new Exception("Wrong expression");
            }
            else
            {
                x = val.intV; 
            }
            return x;

            
            
            
              
            

          
        }

        private static int Priority(StructExpr op)
        {
            

            if (op.Op == OpType.Pow2)
                return 4;
            if (op.Op == OpType.Times || op.Op == OpType.Divide)
                return 3;
            if (op.Op == OpType.Plus || op.Op == OpType.Minus)
                return 2;
            else
                return 1;
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