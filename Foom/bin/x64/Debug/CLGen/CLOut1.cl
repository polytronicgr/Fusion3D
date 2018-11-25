typedef struct __attribute__ ((packed)) tag_rect{
   int x;
   int y;
   int w;
   int h;
}rect;
 
 
void drawRect(  rect r1 )
{
    int dx;
    int dy;
    for( int dy = r1.y;dy<(r1.y+r1.h);dy = dy+1)
  {
    for(dx = r1.x;dx<(r1.x+r1.w);dx = dx+1)
  {
    int loc = (dy*r1.w)*3;
    loc = loc+(dx*3);
  }
  }
}
 
__kernel void imageRender(__global rect *draw , __global unsigned char * dis)
{
  int index = get_global_id(0);
}
