using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
namespace Vivid3D.UI
{
    public class UIItem
    {
        public bool Open = false;
        public string Name = "";
        public VTex2D Img = null;
        public object Link = "";
        public UIItem Top = null;
        public List<UIItem> Sub = new List<UIItem>();
        public UIItem(string name="")
        {
            Name = name;
        }
        public UIItem Add(UIItem i)
        {
            Sub.Add(i);
            i.Top = this;
            return i;
        }
        public void SmartAdd(string top, UIItem i)
        {
            var t = FindSub(top);
            if (t != null)
            {
                t.Add(i);
            }

        }
        public int Count
        {
            get
            {
                return Sub.Count();
            }
        }
        public UIItem FindSub(string name)
        {
            if (Name == name) return this;
            foreach(var s in Sub)
            {
                var ri = FindSub(name);
                if (ri != null) return ri;
            }
            return null;
        }
    }
}
