using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Vivid3D.Audio
{
    public class VividAudio
    {
        public static ISoundOut _Out;
        private static readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
        public static MMDevice mdev;
        public static void Init()
        {

            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        mdev = device;
                        //                       _devices.Add(device);
                        Console.WriteLine("AudioDevice:" + device.FriendlyName);
                    }
                }
            }

            _Out = new WasapiOut()
            {
                Latency = 100,
                Device = mdev
            };

        }
        public static void Play(string file)
        {

            Cur = new AudioSample();
            Cur._Source = CodecFactory.Instance.GetCodec(file)
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();

            _Out.Initialize(Cur._Source);
            _Out.Play();
            //       if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;
        }


        public static AudioSample Cur;
    }

    public class AudioSample
    {
        public IWaveSource _Source;
    }

}
