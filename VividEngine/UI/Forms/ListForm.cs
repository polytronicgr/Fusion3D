using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;
namespace Vivid3D.Resonance.Forms
{
    public class ListForm : UIForm
    {
        public ScrollBarV Scroller = null;
        public List<ItemForm> Items = new List<ItemForm>();
        
        public ListForm()
        {

            PushArea = true;
            Col = new Vector4(0.8f, 0.8f, 0.8f, 0.5f);

            void DrawFunc()
            {

                DrawFormSolid(Col);

            }

            void ChangedFunc()
            {

                Scroller.X = W - 10;
                Scroller.Y = 0;
                Scroller.W = 10;
                Scroller.H = H;
                Scroller.Changed?.Invoke();
                foreach (var item in Items)
                {
                    Forms.Remove(item);
                }

                float sh = (Scroller.H - Scroller.ScrollBut.H);
                float mh = Items.Count * 20;

                float dh = sh / mh;
                float sm1 = 0;
                if (dh < 0.03f)
                {
                    //Scroller.Max = Scr
                //    sm1 = (float)Scroller.H * 0.03f; 
               
                }

                Scroller.ScrollBut.H = (int)(dh * Scroller.H);
                if (dh < 0.1f)
                {

                    //Scroller.ScrollBut.H = 6;
                    //Scroller.Max 
                    //Scroller.Max = Scroller.Max - 10;

                }

                Scroller.Max = Scroller.H;
                float ly = Scroller.Cur / Scroller.Max;
                float mh2 = ly * ((Items.Count+1) * 22);


               
                if (Scroller.ScrollBut.H > H)
                {
                    Scroller.ScrollBut.H = H;
                }
                //ly = -(ly * H);
                ly = -(mh2);
                //ly = ly - 20;

                foreach (var item in Items)
                {
                    //var newi = new ItemForm().Set(5, (int)ly, W - 15, 20, item.Text) as ItemForm;
                    var newi = item;
                    //newi.Pic = item.Pic;
                    if (ly > H-22 || ly<0)
                    {
                        newi.Render = false;
                        newi.CheckBounds = false;
                    }
                    else
                    {
                        newi.CheckBounds = true;
                        newi.Render = true;
                    }
                    newi.Y = (int)ly;
                    ly = ly + 22;
                   
                    Add(newi);
                    
                }

                if (Scroller.ScrollBut.H < 5) Scroller.ScrollBut.H = 5;

                

            }

            Changed = ChangedFunc;

            Draw = DrawFunc;

            Scroller = new ScrollBarV();

            void PostDragFunc(int x,int y)
            {
                //Scroller.Cur = Scroller.ScrollBut.Y / Scroller.H;
                //float my = Scroller.Max / Scroller.H;
                //Scroller.Cur = Scroller.Cur * my;

                Scroller.Cur = Scroller.ScrollBut.Y;
                Changed?.Invoke();
                
            }

            Scroller.ScrollBut.PostDrag = PostDragFunc;

            Add(Scroller);

        }
        public void Clear()
        {
            foreach(var i in Items)
            {
                Forms.Remove(i);
            }
            Items.Clear();
        }

        public ItemForm AddItem(ItemForm item)
        {
            Items.Add(item);
            return item;
            //Changed?.Invoke();
        }

        public ItemForm AddItem(string text,VTex2D pic)
        {
            var nitem = new ItemForm();
            nitem.Text = text;
            nitem.Pic = pic;
            nitem.W = W-20;
            nitem.H = 20;
            Items.Add(nitem);
            return nitem;
            //Changed?.Invoke();
        }

    }
}
