using System;
using VividScript.VStructs;

namespace VividScript
{
    public class VME
    {
        public CodeScope SystemScope = new CodeScope();
        public System.Collections.Generic.Stack<CodeScope> Scopes = new System.Collections.Generic.Stack<CodeScope>();

        public void RegisterOSFuncs ( )
        {
            // - Millisecs()

            CFuncLink millisecs = new CFuncLink
            {
                Link = (t) =>
                {
                    return (dynamic)Environment.TickCount;
                }
            };

            // - printF( <Expressions> )

            CFuncLink printF = new CFuncLink
            {
                Link = ( t ) =>
                {
                    Console.WriteLine ( "printF:" + t[0]);

                    return null;
                }
            };

            AddCFunc ( "printf", printF );
        }

        public static VME Main = null;

        public VSEntry Entry = null;

        public static CodeScope CurrentScope => Main.Scopes.Peek ( );

        public void AddCFunc ( string name, CFuncLink link )
        {
            FuncLink FuncLink = new FuncLink
            {
                Name = name,
                Link = link,
                Type = FuncType.CSharp,
            };
            SystemScope.RegisterFunc ( FuncLink );
        }

        public VME ( )
        {
            Main = this;
            RegisterOSFuncs ( );
        }

        public void Error ( string err, string type = "", VStruct point = null )
        {
            Console.WriteLine ( "Error:->" + err + "@" + type );
        }

        public void Log ( string msg, string type = "", VStruct point = null )
        {
            Console.WriteLine ( "Message:->" + msg + "@" + type );
        }

        public void SetEntry ( VStructs.VSEntry entry )
        {
            Entry = entry;
        }

        public CodeScope RunEntry ( )
        {
            VSFunc entry_func = Entry.FindSystemFunc("Entry");
            if ( entry_func == null )
            {
                Error ( "Unable to find entry point." );
            }
            PushScope ( SystemScope );
            return ExecuteFunc ( entry_func );
            PopScope ( );
        }

        public static void PopScope ( )
        {
            Main.Scopes.Pop ( );
        }

        public static void PushScope ( CodeScope scope )
        {
            if ( Main.Scopes.Count != 0 )
            {
                scope.OutterScope = Main.Scopes.Peek ( );
            }
            else
            {
                scope.OutterScope = null;
            }
            Main.Scopes.Push ( scope );
        }

        public CodeScope ExecuteFunc ( VSFunc func )
        {
            Log ( "Running Func:" + func.Name );

            PushScope ( func.LocalScope );

            func.Code.Exec ( );

            PopScope ( );
            return func.LocalScope;
        }
    }
}