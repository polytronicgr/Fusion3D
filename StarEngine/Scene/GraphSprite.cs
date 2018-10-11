using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Scene
{
    public class GraphSprite : GraphNode
    {
        public GraphSprite(Tex.Tex2D img,int w=-1,int h=-1)
        {
            ImgFrame = img;
            if (w == -1)
            {
                W = ImgFrame.Width;
            }
            else
            {
                W = w;
            }
            if (h == -1)
            {
                H = ImgFrame.Height;
            }
            else
            {
                H = h;
            }
        }
        public GraphSprite(string path,int w=-1,int h=-1)
        {

            ImgFrame = new Tex.Tex2D(path, true);
            if(w==-1)
            {
                W = ImgFrame.Width;
            }
            else
            {
                W = w;
            }
            if (h == -1)
            {
                H = ImgFrame.Height;
            }
            else
            {
                H = h;
            }

        }
    }
}
