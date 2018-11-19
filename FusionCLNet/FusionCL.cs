using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace FusionCLNet
{
    public class FusionCL
    {
        [DllImport("FusionCLNative.dll")]
        public static extern void InitFusionCL();

    }

    public enum BufferType
    {
        Read_Only = (1 << 2),
        Write_Only = (1 << 1),
        Read_Write = (1 << 0),
        Use_Host_Ptr = (1 << 3),
        Alloc_Host_Ptr = (1 << 4),
        Copy_Host_Ptr = (1 << 5),
        Host_Write_Only = (1 << 7),
        Host_Read_Only = (1 << 8),
        Host_No_Access = (1 << 9)

    }

    public class MemBuffer
    {
        int handle;
        public MemBuffer(int bytes,BufferType type)
        {

            handle = CreateBuf(bytes, (int)type, IntPtr.Zero);

        }
        public MemBuffer(BufferType type, byte[] ptr)
        {

            if (ptr != null)
            {
                unsafe
                {
                    fixed (byte* p = ptr)
                    {
                        IntPtr pp = (IntPtr)p;
                        handle = CreateBuf(ptr.Length, (int)type, pp);
                    }
                }


            }
        }
        [DllImport("FusionCLNative.dll")]
        private static extern int CreateBuf(int bytes, int flags, IntPtr ptr);

    }
    public class CommandQueue
    {
        int handle = 0;
        public CommandQueue()
        {
            handle = CreateComQueue();
            Console.WriteLine("ComQueueHandle:" + handle);
        }

        [DllImport("FusionCLNative.dll")]
        private static extern int CreateComQueue();
    }
}
