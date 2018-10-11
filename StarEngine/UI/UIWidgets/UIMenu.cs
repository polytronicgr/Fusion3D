using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.App;

namespace Vivid3D.UI.UIWidgets
{
    public class UIMenu : UIWidget
    {
        public UIMenu(UIWidget top = null) : base(0, 0, AppInfo.W, 25, "", top)
        {
   
        }
        public UIMenuItem AddItem(string text)
        {
            int xs = 0;
            foreach (var i in Sub)
            {
                xs += (int) i.WidW+ 5;
               // i.WidW = UISys.Skin().SmallFont.Width(i.Name) + 5;
                //i.WidH = 25;

            }

            UIMenuItem ni = new UIMenuItem(text, null,true)
            {

                WidX = (int)0,

                WidW = UISys.Skin().SmallFont.Width(text) + 10,
                WidH = 22
            };
            UIPanel np = new UIPanel(10 + xs, 2, (int)ni.WidW, 22, "", this);
            np.Flat = true;
            np.AddWidget(ni);


            return ni;

        }
        public override void AppResize(int w,int h)
        {

            int xs = 0;
            foreach (var i in Sub)
            {
               
                i.WidW = UISys.Skin().SmallFont.Width(i.Sub[0].Name) + 5;
                i.WidH = 25;
                i.WidX = 10 + xs;
                xs += UISys.Skin().SmallFont.Width(i.Sub[0].Name) + 5;

            }
        }
        public override void Draw()
        {
            WidH = 25;
            UISys.Skin().DrawRect(0, 0, (int)WidW,(int)WidH, new OpenTK.Vector4(0.6f, 0.6f, 0.6f, 0.8f));
            
        }
    }
}
