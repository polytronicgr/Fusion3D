using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public enum ExprType
    {
        IntValue, FloatValue, StringValue, RefValue, ClassValue, SubExpr, Operator, Unknown, VarValue, BoolValue,NewClass
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



        public override dynamic Exec()
        {
            if (Expr[0].NewClass)
            {

                var base_class = ManagedHost.Main.FindClass(Expr[0].NewClassType);
                var nvar = new Var();
                nvar.Name = base_class.ModuleName + "_Instnace";

                var bc = base_class.CreateInstance();

                if (Expr[0].NewPars != null)
                {
                    dynamic[] pars = new dynamic[Expr[0].NewPars.Pars.Count];
                    if (Expr[0].NewPars != null)
                    {
                        int ii = 0;
                        foreach (var pe in Expr[0].NewPars.Pars)
                        {
                            pars[ii] = pe.Exec();
                            ii++;
                        }

                        ManagedHost.Main.ExecuteMethod(bc, base_class.ModuleName, pars);
                    }
                }
                else
                {


                    ManagedHost.Main.ExecuteMethod(bc, base_class.ModuleName, null);
                }
                return bc;

            }
            //Val.Clear();
            switch (Expr[0].Type)
            {
                case ExprType.NewClass:
                    Console.WriteLine("NC");
                    break;
                case ExprType.StringValue:

                    string rs = "";
                    foreach(var se in Expr)
                    {
                        switch (se.Type)
                        {
                            case ExprType.StringValue:
                                rs = rs + se.StringV;
                                break;
                            case ExprType.SubExpr:
                                var ser = se.Exec();
                                rs = rs + se.Exec();
                                break;
                            case ExprType.IntValue:
                                rs = rs + se.intV.ToString();
                                break;
                            case ExprType.VarValue:
                                var strv = ManagedHost.CurrentScope.FindVar(se.VarName, true);
                                rs = rs + strv.Value;

                                break;
                        }
                    }
                    return rs;
                    break;
                case ExprType.IntValue:
                case ExprType.VarValue:
                    Stack.Clear();
                    Output.Clear();
                    ToRPNInt(Expr);
                    var res = CalcInt();


                    return res.intV;
                    break;
            }

            return 0;

        }

        public StructExpr CalcInt()
        {
            Stack.Clear();
            for (int i = 0; i < Output.Count; i++)
            {
                if (Output[i].Type == ExprType.Operator)
                {
                    var left = Stack.Pop();
                    var right = Stack.Pop();
                    switch (Output[i].Op)
                    {
                        case OpType.Plus:
                            var ne1 = new StructExpr();
                            ne1.intV = left.intV + right.intV;
                            Stack.Push(ne1);
                            break;
                        case OpType.Times:
                            var ne2 = new StructExpr();
                            ne2.intV = left.intV * right.intV;
                            Stack.Push(ne2);
                            break;
                        case OpType.Divide:
                            var ne3 = new StructExpr();
                            ne3.intV = left.intV / right.intV;
                            Stack.Push(ne3);
                            break;
                        case OpType.Minus:
                            var ne4 = new StructExpr();
                            ne4.intV = left.intV - right.intV;
                            Stack.Push(ne4);
                            break;
                    }
                    //switch(Output[i].)

                }
                else
                {
                    Stack.Push(Output[i]);
                }

            }
            return Stack.Peek();
          
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
                    case ExprType.VarValue:
            
                        var av = ManagedHost.CurrentScope.FindVar(exp[i].VarName, true);
                        var ne = new StructExpr();
                        ne.intV = av.Value;
                        Output.Add(ne);
                        break;
                    case ExprType.SubExpr:
                        var ns = new StructExpr();
                        ns.intV = exp[i].Exec();
                        Output.Add(ns);
                        break;
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