using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace DataCore
{
    public class Data
    {

        public int Bytes
        {
            get
            {
                return _Bytes;
            }

            set
            {
                _Bytes = value;
            }
        }
        protected int _Bytes = 0;

        public byte[] RawData
        {
            get
            {
                return _RawData;
            }
            set
            {
                _RawData = value;
            }
        }
        protected byte[] _RawData = new byte[0];

        public virtual void GenerateBytes()
        {
         
        }

    }
}
