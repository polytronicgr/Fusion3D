using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using OpenTK;
namespace Vivid3D.UI.UIWidgets
{
    public class UIImage : UIWidget
    {
        public VTex2D Img = null;
        public Vector4 Col = Vector4.One;
        public UIImage(int x,int y,int w,int h,VTex2D img,UIWidget top=null) : base(x,y,w,h,"",top)
        {
            Img = img;
        }
        public override void Draw()
        {
            if (Img == null) return;
            UISys.Skin().DrawImg((int)WidX,(int)WidY,(int) WidW, (int)WidH, Img);
        }
    }
}
