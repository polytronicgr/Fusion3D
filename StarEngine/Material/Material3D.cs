using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Tex;
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
        public OpenTK.Vector3 Spec = new OpenTK.Vector3(0.9f, 0.9f, 1.4f);
        public float Shine = 0.8f;
        public static Material3D Active = null;
        public void LoadTexs(string folder,string name)
        {
            TCol = new VTex2D(folder + "//" + name + "_c.png",LoadMethod.Single,false);
            TNorm = new VTex2D(folder + "//" + name + "_n.png",LoadMethod.Single,false);
        }
        public virtual void Bind()
        {
            if(TCol!=null) TCol.Bind(0);

            if (TNorm != null) TNorm.Bind(1);

            if (TEnv != null) TEnv.Bind(2);
           
            Active = this;
        }
        public virtual void Release()
        {
            if (TCol != null) TCol.Release(0);
          
            if(TNorm!=null) TNorm.Release(1);
            if(TEnv!=null) TEnv.Release(2);
            Active = null;
        }
    }
}
