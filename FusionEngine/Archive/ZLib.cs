using System.IO;
using zlib;

namespace Fusion3D.Archive
{
    public class ZLib
    {
        public byte [ ] Compress ( byte [ ] data )
        {
            CompressData ( data, out byte [ ] od );
            return od;
        }

        public byte [ ] Decompress ( byte [ ] data )
        {
            DecompressData ( data, out byte [ ] od );
            return od;
        }

        public static void CompressData ( byte [ ] inData, out byte [ ] outData )
        {
            using ( MemoryStream outMemoryStream = new MemoryStream ( ) )
            using ( ZOutputStream outZStream = new ZOutputStream ( outMemoryStream, zlibConst.Z_DEFAULT_COMPRESSION ) )
            using ( Stream inMemoryStream = new MemoryStream ( inData ) )
            {
                CopyStream ( inMemoryStream, outZStream );
                outZStream.finish ( );
                outData = outMemoryStream.ToArray ( );
            }
        }

        public static void DecompressData ( byte [ ] inData, out byte [ ] outData )
        {
            using ( MemoryStream outMemoryStream = new MemoryStream ( ) )
            using ( ZOutputStream outZStream = new ZOutputStream ( outMemoryStream ) )
            using ( Stream inMemoryStream = new MemoryStream ( inData ) )
            {
                CopyStream ( inMemoryStream, outZStream );
                outZStream.finish ( );
                outData = outMemoryStream.ToArray ( );
            }
        }

        public static void CopyStream ( System.IO.Stream input, System.IO.Stream output )
        {
            byte[] buffer = new byte[2000];
            int len;
            while ( ( len = input.Read ( buffer, 0, 2000 ) ) > 0 )
            {
                output.Write ( buffer, 0, len );
            }
            output.Flush ( );
        }
    }
}