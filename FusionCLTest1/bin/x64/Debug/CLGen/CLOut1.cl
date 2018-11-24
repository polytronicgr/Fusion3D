

__kernel void imageRender(__global int *a, __global int * b)
{
  int index = get_global_id(0);
  b[index] = a[index];
  
}
