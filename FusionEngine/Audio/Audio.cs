using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;
using System.Collections.Generic;

namespace FusionEngine.Audio
{


    public static class StarSoundSys
    {
        private static ISoundEngine SE;

        public static void Init()
        {
            SE = new ISoundEngine();
        }

        public static VSound Play2DFile(string file, bool loop = false)
        {
            return new VSound(SE.Play2D(file, loop));
        }

        public static VSound Play2D(ISoundSource src, bool loop = false)
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
        public static List<VSound> Active = new List<VSound>();

        public VSound(ISound s)
        {
            Active.Add(this);
            Src = s;
            StarSoundSys.SL.Add(this);
        }

        ~VSound()
        {
            Src.Stop();
            Src = null;
        }

        public bool Playing => Src.Finished == false;

        public float Pitch
        {
            set
            {
                Src.PlaybackSpeed = value;
            }
        }
        public float Vol
        {
            set
            {
                Src.Volume = value;
            }
        }

        public void Stop()
        {
            Src.Stop();
        }

        public bool Paused
        {
            get => Src.Paused;
            set => Src.Paused = value;
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

        public VSound Play2D(bool loop = false)
        {
            return StarSoundSys.Play2D(Src, loop);
        }
    }
}