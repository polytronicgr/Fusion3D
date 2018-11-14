using System.Reflection;

namespace Fusion3D.Reflect
{
    public class ClassProperty
    {
        public object Val
        {
            get;
            set;
        }

        public PropertyInfo Prop
        {
            get;
            set;
        }

        public ClassProperty ( object val, PropertyInfo info )
        {
            Val = val;
            Prop = info;
        }
    }
}