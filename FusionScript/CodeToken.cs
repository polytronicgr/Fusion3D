using System.Collections.Generic;
using System.Linq;

namespace FusionScript
{
    public enum TokenClass
    {
        Type, Value, Statement, Define, Op, Flow, Id, Sep, Scope, Array, New, Bool, Assign, BeginLine
    }

    public enum Token
    {
        If, Else, End, Module, Method, Func, Equal, Plus, Minus, Multi, Div, Comma, Peroid, Colon, SemiColon, StringMark, Int, Short, Byte, Long, Float, Double, String,
        Var, Transient, State, Auto, Link, Pow, ElseIf, For, Next, While, Id, LeftPara, RightPara, LeftArray, RightArray, New, Bool, True, False, Greater, Lesser, Not,
        GreatEqual, LessEqual, EndLine, Wend, BeginLine,Percent,Return
    }

    public class CodeToken
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

        public CodeToken Clone ( )
        {
            CodeToken nt = new CodeToken(Class, Token, Text);
            return nt;
        }

        public CodeToken ( TokenClass cls, Token tok, string txt )
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

    public class TokenStream
    {

        
        public List<CodeToken> Tokes
        {
            get => _Toks;
            set
            {
                _Toks = value;
                Len = Tokes.Count ( );
                Pos = 0;
            }
        }

        private List<CodeToken> _Toks = new List<CodeToken>();
        public int Pos = 0;
        public int Len = 0;

     

        public void GotoPrev(Token t)
        {
            for(int i = Pos; i >= 0; i--)
            {
                if(Tokes[i].Token == t)
                {
                    Pos = i;
                    return;
                }
            }
        }

        public void GotoNext(Token t)
        {

            for(int i = Pos; i < Len; i++)
            {
                if (Tokes[i].Token == t)
                {
                    Pos = i;
                    return;
                }
            }

        }

        public void Add ( CodeToken t )
        {
            Tokes.Add ( t );
            Len++;
        }

        public CodeToken Get ( )
        {
            if ( Pos >= Tokes.Count )
            {
                return null;
            }

            return Tokes [ Pos ];
        }

        public CodeToken GetNext ( )
        {
            if ( Pos >= Len )
            {
                return null;
            }

            return Tokes [ Pos++ ];
        }

        public CodeToken Find ( Token t )
        {
            foreach ( CodeToken ti in Tokes )
            {
                if ( ti.Token == t )
                {
                    return ti;
                }
            }
            return null;
        }

        public CodeToken FindPrev ( Token t )
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

        public CodeToken FindNext ( Token t )
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