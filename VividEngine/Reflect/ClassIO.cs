using System;
using System.Collections.Generic;

namespace Vivid3D.Reflect
{
    public class ClassIO
    {
        public object Class
        {
            get;
            set;
        }

        public List<ClassProperty> Props
        {
            get;
            set;
        }

        public ClassIO ( object cls )
        {
            Props = new List<ClassProperty> ( );
            Class = cls;
        }

        public void Copy ( )
        {
            Type t = Class.GetType();

            foreach ( System.Reflection.PropertyInfo p in t.GetProperties ( ) )
            {
                if ( p.CanRead == true && p.CanWrite )
                {
                    object val = p.GetValue(Class);

                    ClassProperty cp = new ClassProperty(val, p);
                    //Console.WriteLine(p.Name + ":" + val);

                    Props.Add ( cp );
                }
            }
        }

        public void Reset ( )
        {
            Type t = Class.GetType();

            foreach ( ClassProperty p in Props )
            {
                Console.WriteLine ( p.Prop.Name + ":" + p.Val );

                p.Prop.SetValue ( Class , p.Val );
            }
        }
    }
}