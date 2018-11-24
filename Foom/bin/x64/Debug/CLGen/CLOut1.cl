typedef struct __attribute__ ((packed)) tag_rect{
   int x;
   int y;
   int w;
   int h;
}rect;
 
typedef struct __attribute__ ((packed)) tag_display{
   int width;
   int height;
   byte *rgb;
}display;
 
 
void drawRect( rect do )
{
    int dx;
    int dy;
    for( int dy = do.y;dy<(do.y+do.h);dy = dy+1)
  {
    for(dx = do.x;dx<(do.x+do.w);dx = dx+1)
  {
    int loc = (dy*do.w)*3;
    loc = loc+(dx*3);
  }
  }
}
 
__kernel void imageRender(__global rect *draw , __global display dis1)
{
  int index = get_global_id(0);
}
