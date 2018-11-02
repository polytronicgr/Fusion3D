namespace Vivid3D.Effect
{
    public class ENoFX : Effect3D
    {
        public ENoFX ( ) : base ( "", "Data/Shader/vsNoFX.glsl", "Data/Shader/fsNoFX.glsl" )
        {
        }

        public override void SetPars ( )
        {
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetTex ( "tC", 0 );
        }
    }

    public class ELightMap : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public ELightMap ( ) : base ( "", "Data/Shader/vsLightMap.glsl", "Data/Shader/fsLightMap.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );

            SetTex ( "tC", 0 );

            //SetTex("tSpec", 2);
        }
    }

    public class ETerrain : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public ETerrain ( ) : base ( "", "Data/Shader/vsTerrain.glsl", "Data/Shader/fsTerrain.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetFloat ( "lightDepth", Settings.Quality.ShadowDepth );
            SetVec3 ( "viewPos", FXG.Cam.LocalPos );
            SetVec3 ( "lightPos", Lighting.GraphLight3D.Active.WorldPos );
            SetVec3 ( "lightCol", Lighting.GraphLight3D.Active.Diff * LightMod );
            SetVec3 ( "lightSpec", Lighting.GraphLight3D.Active.Spec * LightMod );
            SetFloat ( "lightRange", Lighting.GraphLight3D.Active.Range );
            SetFloat ( "atten", Lighting.GraphLight3D.Active.Atten );
            SetVec3 ( "ambCE", Lighting.GraphLight3D.Active.Amb * LightMod );
            SetFloat ( "matS", Material.Material3D.Active.Shine * MatMod );
            SetVec3 ( "matSpec", Material.Material3D.Active.Spec * MatMod );
            SetFloat ( "envS", Material.Material3D.Active.envS * MatMod );
            SetVec3 ( "matDiff", Material.Material3D.Active.Diff * MatMod );
            SetTex ( "tC", 0 );
            SetTex ( "tN", 1 );
            //SetTex("tSpec", 2);

            SetTex ( "tS", 2 );
            SetTex ( "tSpec", 3 );
        }
    }

    public class EMultiPass3D : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public EMultiPass3D ( ) : base ( "", "Data/Shader/vsMP1.glsl", "Data/Shader/fsMP1.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetFloat ( "lightDepth", Settings.Quality.ShadowDepth );
            SetVec3 ( "viewPos", FXG.Cam.LocalPos );
            SetVec3 ( "lightPos", Lighting.GraphLight3D.Active.WorldPos );
            SetVec3 ( "lightCol", Lighting.GraphLight3D.Active.Diff * LightMod );
            SetVec3 ( "lightSpec", Lighting.GraphLight3D.Active.Spec * LightMod );
            SetFloat ( "lightRange", Lighting.GraphLight3D.Active.Range );
            SetFloat ( "atten", Lighting.GraphLight3D.Active.Atten );
            SetVec3 ( "ambCE", Lighting.GraphLight3D.Active.Amb * LightMod );
            SetFloat ( "matS", Material.Material3D.Active.Shine * MatMod );
            SetVec3 ( "matSpec", Material.Material3D.Active.Spec * MatMod );
            SetFloat ( "envS", Material.Material3D.Active.envS * MatMod );
            SetVec3 ( "matDiff", Material.Material3D.Active.Diff * MatMod );
            SetTex ( "tC", 0 );
            SetTex ( "tN", 1 );
            //SetTex("tSpec", 2);

            SetTex ( "tS", 2 );
            SetTex ( "tSpec", 3 );
        }
    }
}