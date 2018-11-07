using System.Collections.Generic;
using System.Linq;

namespace VividScript
{
    public enum TokenClass
    {
        Type, Value, Statement, Define, Op, Flow, Id, Sep, Scope, Array, New, Bool, Assign, BeginLine
    }

    public enum Token
    {
        If, Else, End, Module, Method, Func, Equal, Plus, Minus, Multi, Div, Comma, Peroid, Colon, SemiColon, StringMark, Int, Short, Byte, Long, Float, Double, String,
        Var, Transient, State, Auto, Link, Pow, ElseIf, For, Next, While, Id, LeftPara, RightPara, LeftArray, RightArray, New, Bool, True, False, Greater, Lesser, Not,
        GreatEqual, LessEqual, EndLine, Wend, BeginLine
    }

    public class VToken
    {
        public TokenClass Class;
        public Token Token;
        public string Text = "";
        public string String = "";
        public byte BVal;
        public short SVal;
        public int IVal;
        public long LVal;
        public float FVal;
        public double DVal;

        public VToken Clone ( )
        {
            VToken nt = new VToken(Class, Token, Text);
            return nt;
        }

        public VToken ( TokenClass cls, Token tok, string txt )
        {
            Class = cls;
            Token = tok;
            Text = txt;
        }

        public override string ToString ( )
        {
            return "TokenClass:" + Class.ToString ( ) + " TokenType:" + Token.ToString ( ) + " Text:" + Text;
        }
    }

    public class VTokenStream
    {
        public List<VToken> Tokes
        {
            get => _Toks;
            set
            {
                _Toks = value;
                Len = Tokes.Count ( );
                Pos = 0;
            }
        }

        private List<VToken> _Toks = new List<VToken>();
        public int Pos = 0;
        public int Len = 0;

        public void Add ( VToken t )
        {
            Tokes.Add ( t );
            Len++;
        }

        public VToken Get ( )
        {
            if ( Pos >= Tokes.Count )
            {
                return null;
            }

            return Tokes [ Pos ];
        }

        public VToken GetNext ( )
        {
            if ( Pos >= Len )
            {
                return null;
            }

            return Tokes [ Pos++ ];
        }

        public VToken Find ( Token t )
        {
            foreach ( VToken ti in Tokes )
            {
                if ( ti.Token == t )
                {
                    return ti;
                }
            }
            return null;
        }

        public VToken FindPrev ( Token t )
        {
            for ( int ti = Pos; ti >= 0; ti-- )
            {
                if ( Tokes [ ti ].Token == t )
                {
                    return Tokes [ ti ];
                }
            }
            return null;
        }

        public VToken FindNext ( Token t )
        {
            for ( int ti = Pos; ti < Len; ti++ )
            {
                if ( Tokes [ ti ].Token == t )
                {
                    return Tokes [ ti ];
                }
            }
            return null;
        }
    }
}