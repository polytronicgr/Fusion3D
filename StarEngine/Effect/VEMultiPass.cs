using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Effect
{
    public class EMultiPass3D : Effect3D
    {
        public EMultiPass3D() : base("", "Data/Shader/vsMP1.glsl", "Data/Shader/fsMP1.glsl")
        {

        }
        public override void SetPars()
        {
           
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
            SetFloat("lightDepth", Settings.Quality.ShadowDepth);
            SetVec3("viewPos", FXG.Cam.LocalPos);
            SetVec3("lightPos", Lighting.GraphLight3D.Active.WorldPos);
            SetVec3("lightCol", Lighting.GraphLight3D.Active.Diff);
            SetVec3("lightSpec", Lighting.GraphLight3D.Active.Spec);
            SetFloat("lightRange", Lighting.GraphLight3D.Active.Range);
            SetFloat("atten", Lighting.GraphLight3D.Active.Atten);
            SetFloat("ambCE", Lighting.GraphLight3D.Active.AmbCE);
            SetFloat("matS", Material.Material3D.Active.Shine);
            SetVec3("matSpec", Material.Material3D.Active.Spec);
            SetFloat("envS", Material.Material3D.Active.envS);
            SetTex("tC", 0);
            SetTex("tN", 1);
            SetTex("tS", 2);
        }
    }
}
