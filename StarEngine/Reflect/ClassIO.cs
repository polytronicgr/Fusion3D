using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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

        public ClassIO(object cls)
        {
            Props = new List<ClassProperty>();
            Class = cls;
        }

        public void Copy()
        {

            var t = Class.GetType();

            foreach (var p in t.GetProperties())
            {
                if (p.CanRead == true && p.CanWrite)
                {
                                        var val = p.GetValue(Class);

                    ClassProperty cp = new ClassProperty(val, p);
                    //Console.WriteLine(p.Name + ":" + val);

                    Props.Add(cp);
                }
            }

        }

        public void Reset()
        {

            var t = Class.GetType();

            foreach(var p in Props)
            {
                Console.WriteLine(p.Prop.Name + ":" + p.Val);

                p.Prop.SetValue(Class, p.Val);
               
            }

        }

    }
}
