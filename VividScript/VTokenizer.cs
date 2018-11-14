using System;
using System.Collections.Generic;

namespace VividScript
{
    public enum NextType
    {
        Alpha, Num, Space, Sep, Eol, Unknown, Equal, Greater, Less, Not
    }

    public class VTokenizer
    {
        public List<string> Ops = new List<string>();
        public List<string> Types = new List<string>();
        public List<string> Keys = new List<string>();
        public Dictionary<string, VToken> ConvTable = new Dictionary<string, VToken>();

        public void AddConv ( string tok, VToken t )
        {
            ConvTable [ tok ] = t;
        }

        public void AddConv ( string tok, TokenClass cls, Token t )
        {
            ConvTable [ tok ] = new VToken ( cls, t, tok );
        }

        public VTokenizer ( )
        {
            Ops.Add ( "+" );
            Ops.Add ( "-" );
            Ops.Add ( "/" );
            Ops.Add ( "*" );
            Ops.Add ( "^" );
       
            Ops.Add ( "==" );
            Ops.Add ( "=" );
            AddConv ( "+", new VToken ( TokenClass.Op, Token.Plus, "+" ) );
            AddConv ( "-", new VToken ( TokenClass.Op, Token.Minus, "-" ) );
            AddConv ( "/", new VToken ( TokenClass.Op, Token.Div, "/" ) );
            AddConv ( "*", new VToken ( TokenClass.Op, Token.Div, "*" ) );
            AddConv ( "^", new VToken ( TokenClass.Op, Token.Pow, "^" ) );
        
            AddConv ( "==", new VToken ( TokenClass.Op, Token.Equal, "==" ) );
            AddConv ( "=", new VToken ( TokenClass.Op, Token.Equal, "=" ) );
            AddConv ( "!", new VToken ( TokenClass.Op, Token.Not, "!" ) );
            AddConv ( "<", new VToken ( TokenClass.Op, Token.Lesser, "<" ) );
            AddConv ( ">", new VToken ( TokenClass.Op, Token.Greater, ">" ) );
            AddConv ( "<=", new VToken ( TokenClass.Op, Token.LessEqual, "<=" ) );
            AddConv ( ">=", new VToken ( TokenClass.Op, Token.GreatEqual, ">=" ) );
            AddConv ( "=", new VToken ( TokenClass.Assign, Token.Equal, "=" ) );

            Types.Add ( "byte" );
            Types.Add ( "short" );
            Types.Add ( "int" );
            Types.Add ( "long" );
            Types.Add ( "float" );
            Types.Add ( "double" );
            Types.Add ( "string" );
            Types.Add ( "bool" );
            AddConv ( "byte", TokenClass.Type, Token.Byte );
            AddConv ( "short", TokenClass.Type, Token.Short );
            AddConv ( "int", TokenClass.Type, Token.Int );
            AddConv ( "long", TokenClass.Type, Token.Long );
            AddConv ( "float", TokenClass.Type, Token.Float );
            AddConv ( "double", TokenClass.Type, Token.Double );
            AddConv ( "string", TokenClass.Type, Token.String );
            AddConv ( "bool", TokenClass.Type, Token.Bool );

            Keys.Add ( "if" );
            Keys.Add ( "elseif" );
            Keys.Add ( "end" );
            Keys.Add ( "for" );
            Keys.Add ( "next" );
            Keys.Add ( "while" );
            Keys.Add ( "module" );
            Keys.Add ( "method" );
            Keys.Add ( "function" );
            Keys.Add("var");
            AddConv("var", TokenClass.Define, Token.Var);
            AddConv ( "end", TokenClass.Flow, Token.End );
            AddConv ( "if", TokenClass.Flow, Token.If );
            AddConv ( "elseif", TokenClass.Flow, Token.ElseIf );
            AddConv ( "else", TokenClass.Flow, Token.Else );
            AddConv ( "for", TokenClass.Flow, Token.For );
            AddConv ( "next", TokenClass.Flow, Token.Next );
            AddConv ( "while", TokenClass.Flow, Token.While );
            AddConv ( "wend", TokenClass.Flow, Token.Wend );
            AddConv ( "module", TokenClass.Define, Token.Module );
            AddConv ( "method", TokenClass.Define, Token.Method );
            AddConv ( "function", TokenClass.Define, Token.Func );
            AddConv ( "func", TokenClass.Define, Token.Func );

            Keys.Add ( "," );
            AddConv ( ",", TokenClass.Sep, Token.Comma );

            Keys.Add ( ")" );
            Keys.Add ( "(" );
            Keys.Add ( "[" );
            Keys.Add ( "]" );
            AddConv ( "(", TokenClass.Scope, Token.LeftPara );
            AddConv ( ")", TokenClass.Scope, Token.RightPara );
            AddConv ( "[", TokenClass.Array, Token.LeftArray );
            AddConv ( "]", TokenClass.Array, Token.RightArray );

            Keys.Add ( "new" );
            AddConv ( "new", TokenClass.New, Token.New );

            Keys.Add ( "true" );
            Keys.Add ( "false" );
            AddConv ( "true", TokenClass.Bool, Token.True );
            AddConv ( "false", TokenClass.Bool, Token.False );
        }

        public VToken Id ( string t )
        {
            foreach ( string k in ConvTable.Keys )
            {
                VToken at = ConvTable[k];
                if ( at.Text == t )
                {
                    return at.Clone ( );
                }
            }
            return new VToken ( TokenClass.Id, Token.Id, t );
        }

        public NextType GetNext ( string code, int f )
        {
            f++;
            NextType nt = NextType.Unknown;
            if ( f >= code.Length )

            {
                return NextType.Eol;
            }
            for ( int c = f; f < code.Length; c++ )
            {
                if ( c >= code.Length )
                {
                    break;
                }

                char cc = code[c];
                string cs = cc.ToString();
                if ( cs == " " )
                {
                    return NextType.Space;
                }
                if ( cs == "," )
                {
                    return NextType.Sep;
                }
                if ( cs [ 0 ] >= "a" [ 0 ] && cs [ 0 ] <= "z" [ 0 ] )
                {
                    return NextType.Alpha;
                }
                if ( cs [ 0 ] >= "A" [ 0 ] && cs [ 0 ] <= "Z" [ 0 ] )
                {
                    return NextType.Alpha;
                }
                if ( cs [ 0 ] >= "0" [ 0 ] && cs [ 0 ] <= "9" [ 0 ] )
                {
                    return NextType.Num;
                }
                if ( cs == "=" )
                {
                    return NextType.Equal;
                }
            }
            return nt;
        }

        public VTokenStream ParseString2 ( string code )
        {
            //code = code.Replace ( "   ", "" );

            while ( false )
            {
                int find = code.IndexOf("  ");
                if ( find == -1 )
                {
                    break;
                }

                string code1 = code.Substring ( 0, find );
                string code2 = code.Substring(find+3);
                code1 = code1.Trim ( );
                code2 = code2.Trim ( );
                code = code1 + " " + code2;
            }

            List<string> elements = new List<string>();
            string cur = "";
            bool string_on=false;
            for ( int c = 0; c < code.Length; c++ )
            {
                string ch = code[c].ToString();

                if ( ch == "\"" )
                {
                    if ( !string_on )
                    {
                        string_on = true;
                        if ( cur.Length > 0 )
                        {
                            elements.Add ( cur );
                        }
                        cur = "";
                        continue;
                    }
                    else
                    {
                        elements.Add ( "\"" + cur + "\"" );
                        string_on = false;
                        cur = "";
                        continue;
                    }
                }
                if ( string_on )
                {
                    cur = cur + ch;
                    continue;
                }

                if ( ConvTable.ContainsKey ( cur ) )
                {
                    elements.Add ( cur );
                    cur = "";
                }

                switch ( ch )
                {
                    case "\n":

                        int lg =0;
                        for ( int nn = 0; nn < cur.Length; nn++ )
                        {
                            if ( cur [ nn ].ToString ( ) != " " && cur [ nn ].ToString ( ) != " " )
                            {
                                lg = nn;
                            }
                        }
                        cur = cur.Substring ( 0, lg );

                        break;

                    case " ":
                        if ( cur.Length > 0 )
                        {
                            elements.Add ( cur );
                        }
                        cur = "";
                        continue;
                        break;

                    case "+":
                    case "/":
                    case "-":
                    case "*":
                    case "<":
                    case ">":
                    case "=":
                    case ".":
                    case ",":
                    case ":":
                    case "[":
                    case "]":
                    case "(":
                    case ")":
                    case "!":
                    case "{":
                    case "}":
                    case ";":

                        if ( cur.Length > 0 )
                        {
                            elements.Add ( cur );
                        }

                        elements.Add ( ch );
                        cur = "";

                        continue;
                        break;
                }
                cur += ch;
                // Console.WriteLine ( ch );
            }
            if(cur.Length>0)
            {
                elements.Add(cur);
            }
            List<string> final_elements = new List<string>();
            foreach ( string ele in elements )
            {
                //Console.WriteLine ( "E:" + ele );
                bool keep=false;
                for ( int i = 0; i < good.Length; i++ )
                {
                    if ( ele.Contains ( good [ i ].ToString ( ) ) )
                    {
                        keep = true;
                        continue;
                    }
                }
                string ne = "";
                ne = ele.Trim ( );
                if ( keep )
                {
                    final_elements.Add ( ne );
                }
            }
            VTokenStream rs = new VTokenStream();
            foreach ( string word in final_elements )
            {
                VToken tok = Id(word);
                if ( tok.Text [ 0 ].ToString ( ) == "\"" )
                {
                    tok.Text = tok.Text.Substring ( 1, tok.Text.Length - 2 );
                    tok.Class = TokenClass.Value;
                    tok.Token = Token.String;
                }
                if ( tok.Text == ";" )
                {
                    tok.Class = TokenClass.Flow;
                    tok.Token = Token.EndLine;
                }
                if ( tok.Text [ 0 ] >= "0" [ 0 ] && tok.Text [ 0 ] <= "9" [ 0 ] )
                {
                    tok.Class = TokenClass.Value;
                    tok.Token = Token.Int;
                }
                if ( tok.Text == "=" )
                {
                    tok.Class = TokenClass.Op;
                    tok.Token = Token.Equal;
                }
                //for(int tc = 0l; tc < tok.Token)
                //{
                rs.Add ( tok );
              //  Console.WriteLine ( "Tok:" + tok );
            }

            return rs;
        }

        public string good = "!£$%^&*()-=_+-=/*,.<>[]{}(;:)abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public VTokenStream ParseString ( string code )
        {
            return ParseString2 ( code );
            Console.WriteLine ( "Parsing:" + code );
            VTokenStream ts = new VTokenStream();
            bool stringOn = false;
            string cur = "";
            for ( int c = 0; c < code.Length; c++ )
            {
                char se = code[c];
                if ( stringOn == false )
                {
                    if ( se.ToString ( ) == " " )
                    {
                        if ( cur != " " )
                        {
                            VToken nt = new VToken(TokenClass.Id, Token.Id, cur);

                            ts.Add ( nt );
                        }
                        cur = "";
                    }
                    else
                    {
                        cur = cur + se.ToString ( );
                    }
                }
                else
                {
                    cur = cur + se.ToString ( );
                }
                // Console.WriteLine("Cur:" + cur);

                if ( stringOn == false )
                {
                    if ( cur == " " )
                    {
                        cur = "";
                    }
                }

                if ( cur.Length > 0 )
                {
                    if ( cur == "\"" || cur [ cur.Length - 1 ].ToString ( ) == "\"" )
                    {
                        if ( !stringOn )
                        {
                            cur = "";
                            stringOn = true;
                        }
                        else
                        {
                            VToken nt = new VToken(TokenClass.Value, Token.String, cur.Substring(0, cur.Length - 1));
                            stringOn = false;
                            ts.Add ( nt );
                            cur = "";
                            // Console.WriteLine("Yep");
                        }
                    }
                }
                if ( cur == "+" )
                {
                    VToken nt = new VToken(TokenClass.Op,Token.Plus,"+");
                    ts.Add ( nt );
                    cur = "";
                    continue;
                }
                if ( cur == "-" )
                {
                    VToken nt = new VToken(TokenClass.Op,Token.Minus,"-");
                    ts.Add ( nt );
                    cur = "";
                    continue;
                }
                if ( cur == "*" )
                {
                    VToken nt = new VToken(TokenClass.Op,Token.Multi,"*");
                    ts.Add ( nt );
                    cur = "";
                    continue;
                }
                if ( cur == "/" )
                {
                    VToken nt = new VToken(TokenClass.Op,Token.Div,"/");
                    ts.Add ( nt );
                    cur = "";
                    continue;
                }

                if ( stringOn )
                {
                    continue;
                }

                VToken tt = Id(cur);

                VToken t2 = null;
                if ( cur.Length > 1 )
                {
                    t2 = Id ( se.ToString ( ) );
                }

                if ( "[]()".Contains ( tt.Text ) )
                {
                    t2 = tt;
                    tt = null;
                }

                if ( t2 != null )
                {
                    // Console.WriteLine("t2" + t2.Text);
                    switch ( t2.Token )
                    {
                        case Token.Greater:
                            tt.Text = tt.Text.Substring ( 0, tt.Text.Length - 1 );
                            ts.Add ( tt );

                            cur = "";
                            if ( GetNext ( code, c ) == NextType.Equal )
                            {
                                tt = new VToken ( TokenClass.Op, Token.GreatEqual, ">=" );
                                ts.Add ( tt );
                                c++;
                                continue;
                            }
                            else
                            {
                                tt = new VToken ( TokenClass.Op, Token.Greater, ">" );
                                ts.Add ( tt );
                                continue;
                            }
                            break;

                        case Token.Lesser:
                            tt.Text = tt.Text.Substring ( 0, tt.Text.Length - 1 );
                            ts.Add ( tt );
                            cur = "";
                            if ( GetNext ( code, c ) == NextType.Equal )
                            {
                                tt = new VToken ( TokenClass.Op, Token.LessEqual, "<=" );
                                ts.Add ( tt );
                                c++;
                                continue;
                            }
                            else
                            {
                                tt = new VToken ( TokenClass.Op, Token.Lesser, "<" );
                                ts.Add ( tt );
                                continue;
                            }
                            break;

                        case Token.Equal:

                            ts.Add ( tt );
                            cur = "";
                            if ( GetNext ( code, c ) == NextType.Equal )
                            {
                                tt = new VToken ( TokenClass.Op, Token.Equal, "==" );
                                ts.Add ( tt );
                                c++;
                                continue;
                            }

                            break;

                        case Token.Plus:
                        case Token.Minus:
                        case Token.Div:
                        case Token.Multi:
                        case Token.Pow:

                            cur = cur.Substring ( 0, cur.Length - 1 );
                            tt = new VToken ( TokenClass.Id, Token.Id, cur );
                            ts.Add ( tt );
                            tt = new VToken ( TokenClass.Op, t2.Token, t2.Text );
                            ts.Add ( tt );
                            tt = null;
                            cur = "";

                            break;

                        case Token.LeftPara:
                        case Token.RightPara:
                            // Console.WriteLine("Cur===" + cur + "|");
                            cur = cur.Substring ( 0, cur.Length - 1 );
                            tt = new VToken ( TokenClass.Id, Token.Id, cur )
                            {
                                Class = TokenClass.Id
                            };
                            ts.Add ( tt );
                            Token tok2 = Token.LeftPara;
                            if ( t2.Token == Token.RightPara )
                            {
                                tok2 = Token.RightPara;
                            }

                            tt = new VToken ( TokenClass.Scope, tok2, t2.Text );
                            ts.Add ( tt );
                            tt = null;
                            cur = "";
                            break;

                        case Token.LeftArray:
                        case Token.RightArray:
                            cur = cur.Substring ( 0, cur.Length - 1 );
                            tt = new VToken ( TokenClass.Id, Token.Id, cur )
                            {
                                Class = TokenClass.Id
                            };
                            ts.Add ( tt );
                            Token tok = Token.LeftArray;
                            if ( t2.Token == Token.RightArray )
                            {
                                tok = Token.RightArray;
                            }

                            tt = new VToken ( TokenClass.Array, tok, t2.Text );
                            ts.Add ( tt );
                            tt = null;
                            cur = "";
                            break;

                        case Token.Comma:
                            cur = cur.Substring ( 0, cur.Length - 1 );
                            tt = new VToken ( TokenClass.Id, Token.Id, cur )
                            {
                                Class = TokenClass.Id
                            };
                            ts.Add ( tt );
                            tt = null;
                            cur = "";
                            break;
                    }
                }

                if ( cur == " " )
                {
                    cur = "";
                }
                if ( tt != null && cur != " " )
                {
                    NextType nt = GetNext(code, c);
                    switch ( nt )
                    {
                        case NextType.Alpha:
                            break;

                        case NextType.Space:
                            ts.Add ( tt );
                            cur = "";
                            break;

                        case NextType.Sep:
                            ts.Add ( tt );
                            cur = "";
                            break;
                    }
                }
            }

            if ( cur != "" )
            {
                VToken t2 = Id(cur);
                if ( t2 == null )
                {
                    t2 = new VToken ( TokenClass.Id, Token.Var, cur );
                }

                ts.Add ( t2 );
            }
            VTokenStream ns = new VTokenStream();
            foreach ( VToken at in ts.Tokes )
            {
                if ( at.Text.Length > 0 )
                {
                    if ( at.Text.Substring ( 0, 1 ) == "," )
                    {
                        if ( at.Text.Length > 1 )
                        {
                            at.Text = at.Text.Substring ( 1, at.Text.Length - 1 );
                        }
                    }
                }

                if ( at.Text == " " || at.Text == "" )
                {
                    continue;
                }
                if ( at.Text == "end" || at.Text == "End" )
                {
                    at.Token = Token.End;
                    at.Class = TokenClass.Flow;
                }
                if ( at.Class == TokenClass.Id )
                {
                    if ( at.Text [ 0 ] >= "0" [ 0 ] && at.Text [ 0 ] <= "9" [ 0 ] )
                    {
                        at.Class = TokenClass.Value;
                        at.Token = Token.Int;
                        if ( at.Text.Contains ( "." ) )
                        {
                            at.Token = Token.Double;
                            if ( at.Text.Contains ( "f" ) )
                            {
                                at.Token = Token.Float;
                            }
                        }
                    }
                }
                ns.Add ( at );
            }
            List<VToken> finalstream = new List<VToken>();
            foreach ( VToken n in ns.Tokes )
            {
                TokenClass tc = n.Class;
                Token nt = n.Token;
                string text = n.Text;

                switch ( n.Text )
                {
                    case "int":
                        nt = Token.Int;
                        tc = TokenClass.Type;
                        break;

                    case "float":
                        nt = Token.Float;
                        tc = TokenClass.Type;
                        break;

                    case "string":
                        nt = Token.String;
                        tc = TokenClass.Type;
                        break;

                    case "double":
                        nt = Token.Double;
                        tc = TokenClass.Type;
                        break;
                }
                VToken newtok = new VToken(n.Class,nt,n.Text);
                finalstream.Add ( newtok );
            }
            VTokenStream newtokstr = new VTokenStream
            {
                Tokes = finalstream
            };
            return newtokstr;
        }
    }
}