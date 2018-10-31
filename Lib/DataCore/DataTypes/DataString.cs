using System.Text;

namespace DataCore.DataTypes
{
    public class DataString : Data
    {
        public string String
        {
            get;
            set;
        }

        public DataString ( )
        {
        }

        public DataString ( string data )
        {
            String = data;
        }

        public override void GenerateBytes ( )
        {
            _RawData = Encoding.ASCII.GetBytes ( String );
            _Bytes = _RawData.Length;
        }

        public override void Reconstruct ( )
        {
            String = Encoding.ASCII.GetString ( _RawData );
        }
    }
}