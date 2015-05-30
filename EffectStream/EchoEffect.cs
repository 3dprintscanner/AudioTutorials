using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectStream
{
    class EchoEffect : IEffect
    {
        public int EchoLength { get; private set; }
        public float EchoFactor { get; private set; }
        private Queue<float> samples;
        public EchoEffect(int echoLength=150, float echoFactor=5)
        {
            this.EchoLength = echoLength;
            this.EchoFactor = echoFactor;
            this.samples = new Queue<float>();

            for (int i = 0; i < EchoLength; i++)
            {
                samples.Enqueue(0f);
            }
        }

        public float ApplyEffect(float sample)
        {
            samples.Enqueue(sample);
            return Math.Min(1, Math.Max(-1, sample + EchoFactor*samples.Dequeue()));
        }

        

        
    }
}
