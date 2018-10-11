using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.UI
{
    public enum EventType
    {
        Click,Pressed,Activated,Deactivated,AddedNode,ChangedNode,ClearedNodes,None
    }
    public class UIEvent
    {
        public UIWidget Owner = null;
        public EventType Type = EventType.None;
        public DateTime Time = DateTime.Now;
        public UIEvent(UIWidget w,EventType et)
        {
            Owner = w;
            Type = et;
        }
    }
}
