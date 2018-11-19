using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

    public enum CLBufferType
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

    public class CLMemBuffer
    {
        public int handle;
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
            }
        }
        int _Size = 0;
        public CLMemBuffer(int bytes, CLBufferType type)
        {

            handle = CreateBuf(bytes, (int)type, IntPtr.Zero);
            _Size = bytes;
        }
        public CLMemBuffer(CLBufferType type, byte[] ptr)
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
            _Size = ptr.Length;
        }
        [DllImport("FusionCLNative.dll")]
        private static extern int CreateBuf(int bytes, int flags, IntPtr ptr);

    }
    public class CLCommandQueue
    {
        public int handle = 0;
        public CLCommandQueue()
        {
            handle = CreateComQueue();
            Console.WriteLine("ComQueueHandle:" + handle);
        }

        public byte[] Read(CLMemBuffer mem,bool block,int size=-1)
        {
            if (size == -1)
            {
                size = mem.Size;
            }
            byte[] data = new byte[size];
            unsafe
            {
                fixed(byte *p = data)
                {
                    IntPtr pp = (IntPtr)p;
                    QueueReadBuffer(handle, mem.handle, block, size, pp);
                }
            }
            return data;
        }

        public void Write(CLMemBuffer mem, bool blocking, int offset = 0, int size = -1)
        {

            if (size == -1)
            {
                size = mem.Size;
            }
            //        if(size)

            QueueWriteBuffer(handle, mem.handle, blocking, offset, size, IntPtr.Zero);


        }

        public void Write(CLMemBuffer mem, bool blocking, byte[] data, int offset = 0, int size = -1)
        {
            if (size == -1)
            {
                size = data.Length;
            }
            unsafe
            {
                fixed (byte* pp = data)
                {
                    IntPtr bp = (IntPtr)pp;
                    QueueWriteBuffer(handle, mem.handle, blocking, offset, size, bp);
                }

            }


        }

        public bool Range(CLKernel kernel,int total, int sub)
        {
            return ExecRange(handle, kernel.handle, total, sub);
        }

        [DllImport("FusionCLNative.dll")]
        public static extern bool ExecRange(int queue, int kern, int global, int sub);

       [DllImport("FusionCLNative.dll")]
        private static extern int CreateComQueue();

        [DllImport("FusionCLNative.dll")]
        private static extern bool QueueWriteBuffer(int queue, int mem, bool blocking, int offset, int size, IntPtr ptr);

        [DllImport("FusionCLNative.dll")]
        private static extern bool QueueReadBuffer(int queue, int mem, bool block, int size, IntPtr ptr);

    }
    public static class FUtil
    {
        public static IntPtr ToCString(this String str)
        {

            return Marshal.StringToHGlobalAnsi(str);

        }
        public static void FreeCString(IntPtr cstr)
        {
            Marshal.FreeHGlobal(cstr);
        }
    }

    public class CLKernel
    {
        public int handle;

       

        public bool SetArg(int index, int value)
        {

            unsafe
            {

                int* ptr = &value;
                var ip = new IntPtr(ptr);
                return KernSetArgPtr(handle, index, sizeof(int), ip);

            }

        }

        public bool SetArg(int index,float v)
        {

            unsafe
            {

                float* ptr = &v;
                var ip = new IntPtr(ptr);
                return KernSetArgPtr(handle, index, sizeof(float), ip);
            }


        }

        public bool SetArg(int index,bool v)
        {

            unsafe
            {

                bool* bp = &v;
                IntPtr ip = new IntPtr(bp);
                return KernSetArgPtr(handle, index, sizeof(bool), ip);

            }

        }

        public bool SetArg(int index,CLMemBuffer mem)
        {

            return KernSetArgMem(handle, index, mem.Size, mem.handle);

        }

        [DllImport("FusionCLNative.dll")]
        public static extern bool KernSetArgMem(int kern, int par, int size, int mem);

        [DllImport("FusionCLNative.dll")]
        public static extern bool KernSetArgPtr(int kern, int par, int size, IntPtr p);
    }

    public class CLProgram
    {
        public int handle;

        public CLProgram(string path)
        {
            var src = File.ReadAllText(path);

            int size = src.Length + 1;

            var src_c = src.ToCString();

            handle = CreateProgram(src_c, size);

        }

        public bool Build()
        {
            bool res = BuildProgram(handle);

            return res;
        }

        public CLKernel CreateKernel(string name)
        {
            CLKernel k = new CLKernel();
            k.handle = CreateKern(handle, name.ToCString());
            return k;
        }

        [DllImport("FusionCLNative.dll")]
        public static extern int CreateProgram(IntPtr source, int size);

        [DllImport("FusionCLNative.dll")]
        public static extern bool BuildProgram(int prog);

        [DllImport("FusionCLNative.dll")]
        public static extern int CreateKern(int prog, IntPtr name);
    }
    

}
