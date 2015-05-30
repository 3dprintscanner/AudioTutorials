using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace EffectStream
{
    class EffectsStream : WaveStream
    {

        public List<IEffect> Effects { get; private set; }
 
        public WaveStream SourceStream
        {
            get; set; }

        public EffectsStream(WaveStream stream)
        {

            this.SourceStream = stream;
            this.Effects = new List<IEffect>();
        }   

        public override WaveFormat WaveFormat
        {
            get { return SourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return SourceStream.Length; }
        }

        public override long Position
        {
            get { return SourceStream.Position; }
            set { SourceStream.Position = value; }
        }

        private int channel = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            Console.WriteLine("DirectSoundOut requested {0} bytes", count);
            int read =  SourceStream.Read(buffer, offset, count);

            for (int i = 0; i < read/4; i++)
            {
                float sample = BitConverter.ToSingle(buffer, i * 4);
                if (Effects.Count == WaveFormat.Channels)
                {
                    sample = Effects[channel].ApplyEffect(sample);
                    channel = (channel + 1) % WaveFormat.Channels;
                    }
                sample = sample * 0.5f;
                byte[] bytes = BitConverter.GetBytes(sample);
                //bytes.CopyTo(buffer, i * 4);
                buffer[i * 4 + 0] = bytes[0];
                buffer[i * 4 + 1] = bytes[1];
                buffer[i * 4 + 2] = bytes[2];
                buffer[i * 4 + 3] = bytes[3];

                
            }
            return read;
        }
    }
}
