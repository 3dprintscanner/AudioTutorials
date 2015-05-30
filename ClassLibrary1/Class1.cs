using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ClassLibrary1
{
    public class AudioStream :IDisposable
    {
        private IWavePlayer wavePlayer;
        private WaveStream waveStream;

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
