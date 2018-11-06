using System.Speech.Synthesis;

namespace Vivid3D.Speech
{
    public class SpeechOut
    {
        private SpeechSynthesizer Synth;

        public SpeechOut ( )
        {
            Synth = new SpeechSynthesizer
            {
                Volume = 80,
                Rate = 0
            };
        }

        public void Say ( string text, bool Async = true )
        {
            if ( !Async )
            {
                Synth.Speak ( text );
            }
            else
            {
                Synth.SpeakAsync ( text );
            }
        }
    }
}