using System;
using System.Collections.Generic;

class Rpn
{
    public static void Main()
    {
        char[] sp = new char[] { ' ', '\t' };
        for (; ; )
        {
            string s = Console.ReadLine();
            if (s == null) break;
            Stack<string> tks = new Stack<string>
                 (s.Split(sp, StringSplitOptions.RemoveEmptyEntries));
            if (tks.Count == 0) continue;
            try
            {
                double r = evalrpn(tks);
                if (tks.Count != 0) throw new Exception();
                Console.WriteLine(r);
            }
            catch (Exception e) { Console.WriteLine("error"); }
        }
    }

    private static double evalrpn(Stack<string> tks)
    {
        string tk = tks.Pop();
        double x, y;
        if (!Double.TryParse(tk, out x))
        {
            y = evalrpn(tks); x = evalrpn(tks);
            if (tk == "+") x += y;
            else if (tk == "-") x -= y;
            else if (tk == "*") x *= y;
            else if (tk == "/") x /= y;
            else throw new Exception();
        }
        return x;
    }
}