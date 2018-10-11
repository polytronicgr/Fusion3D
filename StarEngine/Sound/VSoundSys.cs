using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;

namespace Vivid3D.Sound
{
    public static class StarSoundSys
    {
        static ISoundEngine SE;
        public static void Init()
        {
            SE = new ISoundEngine();
        }
        public static VSound Play2DFile(string file,bool loop =false)
        {
            return new VSound(SE.Play2D(file, loop));

        }
        public static VSound Play2D(ISoundSource src,bool loop =false)
        {
            
            return new VSound(SE.Play2D(src, loop, false, false));

        }
        public static void Update()
        {
            SE.Update();
        }
        public static void StopAll()
        {
            SE.StopAllSounds();
        }
        public static ISoundSource LoadSound(string p)
        {
            return SE.AddSoundSourceFromFile(p);
        }
        public static List<VSoundSource> SSL = new List<VSoundSource>();
        public static List<VSound> SL = new List<VSound>();
    }
    public class VSound
    {

        public ISound Src;
        public VSound(ISound s)
        {
            Src = s;
            StarSoundSys.SL.Add(this);
        }
        ~VSound()
        {
            Src.Stop();
            Src = null;
        }
        public bool Playing
        {
            get
            {
                return Src.Finished == false;
            }
        }
        public void Stop()
        {
            Src.Stop();
        }
        public bool Paused
        {
            get
            {
                return Src.Paused;
            }
            set
            {
                Src.Paused = value;
            }
        } 
       
    }
    public class VSoundSource
    {
        public ISoundSource Src;
        public VSoundSource(string p)
        {
            Src = StarSoundSys.LoadSound(p);
            StarSoundSys.SSL.Add(this);
        }
        public VSound Play2D(bool loop=false)
        {
            return StarSoundSys.Play2D(Src, loop);

        }
    }
}
