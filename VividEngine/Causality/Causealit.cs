﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Vivid3D.Logic
{
    public delegate void Act ( );

    public delegate bool Until ( );

    public delegate void FlowInit ( );

    public delegate bool FlowLogic ( );

    public delegate bool When ( );

    public delegate bool Unless ( );

    public delegate bool If ( );

    public class Logics
    {
        public int inter;
        public bool Threaded = false;
        public Thread UpdateThread = null;

        public Logics ( int interval = 1000 / 60 , bool threaded = false )
        {
            inter = interval;
            if ( threaded )
            {
                Threaded = true;
                UpdateThread = new Thread ( new ThreadStart ( Thread_Update ) )
                {
                    Priority = ThreadPriority.Normal
                };
                UpdateThread.Start ( );
            }
        }

        public Mutex upmut = new Mutex();

        public void Thread_Update ( )
        {
            int last = Environment.TickCount;
            int next = last;
            while ( true )
            {
                int ct = Environment.TickCount;
                if ( ct >= next )
                {
                    InternalUpdate ( );
                    next = ct + inter;
                }
                Thread.Sleep ( 2 );
            }
        }

        public virtual void In ( Act Action , Until Until )
        {
            ActInfo ai = new ActInfo
            {
                Action = Action ,
                Until = Until ,
                NoTime = true
            };
            Acts.Add ( ai );
        }

        public virtual void In ( int ms , Act Action , Until Until )
        {
            ActInfo ai = new ActInfo
            {
                Action = Action ,
                When = Environment.TickCount + ms ,
                Until = Until
            };
            Acts.Add ( ai );
        }

        public virtual void In ( int ms , Act Action , bool once = true , int forms = 0 )
        {
            ActInfo ai = new ActInfo
            {
                Action = Action ,
                When = Environment.TickCount + ms ,
                For = forms ,
                Once = once
            };
            Acts.Add ( ai );
        }

        public void Flow ( FlowInit init , FlowLogic logic , Act endLogic = null )
        {
            FlowInfo flow = new FlowInfo
            {
                Init = init ,
                Logic = logic ,
                EndLogic = endLogic
            };
            Flows.Add ( flow );
        }

        public void SmartUpdate ( )
        {
            if ( Threaded )
            {
            }
            else
            {
                InternalUpdate ( );
            }
        }

        public void InternalUpdate ( )
        {
            upmut.WaitOne ( );
            List<IfInfo> iil = new List<IfInfo> ( );
            foreach ( IfInfo ci in Ifs )
            {
                if ( ci.If ( ) )
                {
                    ci.Action ( );
                }
                else
                {
                    if ( ci.Else != null )
                    {
                        ci.Else ( );
                    }
                }
                if ( ci.Until != null )
                {
                    if ( ci.Until ( ) )
                    {
                        iil.Add ( ci );
                    }
                }
            }
            foreach ( IfInfo ci in iil )
            {
                Ifs.Remove ( ci );
            }
            List<DoInfo> rd = new List<DoInfo> ( );
            List<DoInfo> dt = new List<DoInfo> ( );
            foreach ( DoInfo Do in Dos )
            {
                Do.Do ( );
                if ( Do.Until != null )
                {
                    if ( Do.Until ( ) )
                    {
                        if ( Do.Then != null )
                        {
                            dt.Add ( Do );
                        }

                        rd.Add ( Do );
                    }
                }
            }
            foreach ( DoInfo dd in dt )
            {
                dd.Then ( );
            }
            foreach ( DoInfo Do in rd )
            {
                Dos.Remove ( Do );
            }
            List<WhenInfo> rw = new List<WhenInfo> ( );
            foreach ( WhenInfo w in Whens )
            {
                if ( w.When ( ) )
                {
                    if ( w.Unless != null )
                    {
                        if ( w.Unless ( ) )
                        {
                            //rw.Add(w);
                        }
                        else
                        {
                            w.Action ( );
                            rw.Add ( w );
                        }
                    }
                    else
                    {
                        w.Action ( );
                        rw.Add ( w );
                    }
                }
            }
            foreach ( WhenInfo w in rw )
            {
                Whens.Remove ( w );
            }
            if ( Flows.Count > 0 )
            {
                FlowInfo ff = Flows[0];
                if ( ff.Begun == false )
                {
                    if ( ff.Init != null )
                    {
                        ff.Init ( );
                    }
                    ff.Begun = true;
                }
                if ( ff.Logic ( ) )
                {
                    if ( ff.EndLogic != null )
                    {
                        ff.EndLogic ( );
                    }
                    Flows.Remove ( ff );
                }
            }
            List<ActInfo> rem = new List<ActInfo>();
            int ms = Environment.TickCount;
            foreach ( ActInfo a in Acts )
            {
                if ( a.NoTime )
                {
                    a.Action ( );
                    if ( a.Until ( ) )
                    {
                        rem.Add ( a );
                        continue;
                    }
                }
                if ( a.Until != null )
                {
                    if ( ms > ( a.When ) )
                    {
                        a.Action ( );
                        if ( a.Until ( ) )
                        {
                            rem.Add ( a );
                            continue;
                        }
                    }
                }
                if ( a.Running )
                {
                    if ( ms > ( a.When + a.For ) )
                    {
                        Console.WriteLine ( "Done" );
                        rem.Add ( a );
                        continue;
                    }
                }
                else
                {
                    if ( ms > a.When )
                    {
                        a.Action ( );
                        if ( a.Once )
                        {
                            rem.Add ( a );
                        }
                        else
                        {
                            a.Running = true;
                        }
                    }
                }
            }
            foreach ( ActInfo a in rem )
            {
                Acts.Remove ( a );
            }
            upmut.ReleaseMutex ( );
        }

        public void When ( When when , Act action , Unless unless = null )
        {
            foreach ( WhenInfo cw in Whens )
            {
                if ( cw.When == when && cw.Action == action && cw.Unless == unless )
                {
                    return;
                }
            }
            WhenInfo wi = new WhenInfo
            {
                When = when ,
                Action = action ,
                Unless = unless
            };
            Whens.Add ( wi );
        }

        public void Do ( Act action , Until until = null , Act then = null )
        {
            DoInfo di = new DoInfo
            {
                Do = action ,
                Until = until ,
                Then = then
            };
            Dos.Add ( di );
        }

        public void If ( If ifact , Act action , Act Else = null , Until until = null )
        {
            IfInfo ni = new IfInfo
            {
                If = ifact ,
                Else = Else ,
                Action = action ,
                Until = until
            };
            Ifs.Add ( ni );
        }

        public List<IfInfo> Ifs = new List<IfInfo>();
        public List<DoInfo> Dos = new List<DoInfo>();
        public List<FlowInfo> Flows = new List<FlowInfo>();
        private List<ActInfo> Acts = new List<ActInfo>();
        public List<WhenInfo> Whens = new List<WhenInfo>();
    }

    public class IfInfo
    {
        public If If = null;
        public Act Else = null;
        public Act Action = null;
        public Until Until = null;
    }

    public class FlowInfo
    {
        public FlowInit Init;
        public FlowLogic Logic;
        public Act EndLogic = null;
        public bool Begun = false;
    }

    public class DoInfo
    {
        public Act Do = null;
        public Until Until = null;
        public Act Then = null;
    }

    public class ActInfo
    {
        public Act Action;
        public int When;
        public int For;
        public int Begun;
        public bool Running = false;
        public bool Once = true;
        public Until Until = null;
        public bool NoTime = false;
    }

    public class WhenInfo
    {
        public When When = null;
        public Act Action = null;
        public Unless Unless = null;
    }
}