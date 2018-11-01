using Vivid3D.Texture;

namespace Vivid3D.Material
{
    public class Material3D
    {
        public VTex2D TCol;
        public VTex2D TNorm;
        public VTex2D TSpec;
        public VTex2D TAO;
        public VTexCube TEnv;
        public float envS = 0.1f;
        public OpenTK.Vector3 Diff = new OpenTK.Vector3(1, 1, 1);
        public OpenTK.Vector3 Spec = new OpenTK.Vector3(0.3f, 0.3f, 0.3f);
        public float Shine = 2.0f;
        public static Material3D Active = null;

        public Material3D ( )
        {
            TNorm = new Texture.VTex2D ( "data\\tex\\normblank.png" , Texture.LoadMethod.Single , false );
            TCol = new Texture.VTex2D ( "data\\tex\\diffblank.png" , Texture.LoadMethod.Single , false );
            TSpec = new Texture.VTex2D ( "data\\tex\\specblank.png" , Texture.LoadMethod.Single , false );
        }

        public void Write ( )
        {
            Help.IOHelp.WriteVec ( Diff );
            Help.IOHelp.WriteVec ( Spec );
            Help.IOHelp.WriteFloat ( Shine );
            Help.IOHelp.WriteBool ( TCol != null );
            if ( TCol != null )
            {
                TCol.Write ( );
            }
            Help.IOHelp.WriteBool ( TNorm != null );
            if ( TNorm != null )
            {
                TNorm.Write ( );
            }
            Help.IOHelp.WriteBool ( TSpec != null );
            if ( TSpec != null )
            {
                TSpec.Write ( );
            }
        }

        public void Read ( )
        {
            //   Console.WriteLine ( "Thread:" + System.Threading.Thread.CurrentThread.Name + System.Threading.Thread.CurrentThread );
            Diff = Help.IOHelp.ReadVec3 ( );
            Spec = Help.IOHelp.ReadVec3 ( );
            Shine = Help.IOHelp.ReadFloat ( );
            if ( Help.IOHelp.ReadBool ( ) )
            {
                TCol = new VTex2D ( );
                TCol.Read ( );
            }
            if ( Help.IOHelp.ReadBool ( ) )
            {
                TNorm = new VTex2D ( );
                TNorm.Read ( );
            }
            if ( Help.IOHelp.ReadBool ( ) )
            {
                TSpec = new VTex2D ( );
                TSpec.Read ( );
            }
        }

        public void LoadTexs ( string folder , string name )
        {
            TCol = new VTex2D ( folder + "//" + name + "_c.png" , LoadMethod.Single , false );
            TNorm = new VTex2D ( folder + "//" + name + "_n.png" , LoadMethod.Single , false );
        }

        public virtual void BindLightmap ( )
        {
            if ( TCol != null )
            {
                TCol.Bind ( 0 );
            }

            Active = this;
        }

        public virtual void ReleaseLightmap ( )
        {
            if ( TCol != null )
            {
                TCol.Release ( 0 );
            }

            Active = null;
        }

        public virtual void Bind ( )
        {
            if ( TCol != null )
            {
                TCol.Bind ( 0 );
            }

            if ( TNorm != null )
            {
                TNorm.Bind ( 1 );
            }

            //if (TSpec != null) TSpec.Bind(2);

            if ( TEnv != null )
            {
                TEnv.Bind ( 2 );
            }

            if ( TSpec != null )
            {
                TSpec.Bind ( 3 );
            }

            Active = this;
        }

        public virtual void Release ( )
        {
            if ( TCol != null )
            {
                TCol.Release ( 0 );
            }

            if ( TNorm != null )
            {
                TNorm.Release ( 1 );
            }
            //if (TSpec != null) TSpec.Release(2);
            if ( TEnv != null )
            {
                TEnv.Release ( 2 );
            }

            if ( TSpec != null )
            {
                TSpec.Release ( 3 );
            }

            Active = null;
        }
    }
}