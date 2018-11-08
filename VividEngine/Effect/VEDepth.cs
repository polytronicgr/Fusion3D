namespace Vivid3D.Effect
{
    public class EDepth3D : Effect3D
    {
        public EDepth3D ( ) : base ( "", "data/Shader/vsDepth.txt", "data/Shader/fsDepth.txt" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetVec3 ( "camP", FXG.Cam.WorldPos );
            SetFloat ( "minZ", FXG.Cam.MinZ );
            SetFloat ( "maxZ", FXG.Cam.MaxZ );
        }
    }
}