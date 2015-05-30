using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NAudio.Wave;


namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
    public partial class NewWaveForm : Window
    {
        private DirectSoundOut output = null;
        private BlockAlignReductionStream waveStream = null;
        
        public NewWaveForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WaveTone tone = new WaveTone(2000,3);
            waveStream = new BlockAlignReductionStream(tone);
            
            output = new DirectSoundOut();
            output.Init(waveStream);
            output.Play();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (output.PlaybackState == PlaybackState.Playing)
            {
                output.Stop();
            }
        }

        
    }

    public class WaveTone : WaveStream
    {
        private readonly double frequency;
        private readonly double amplitude;
        private double time;

        public override WaveFormat WaveFormat
        {
            get { return new WaveFormat(); }
        }

        public WaveTone(double frequency, double amplitude)
        {
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.time = 0;
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int samples = count/2;
            for (int i = 0; i < samples; i++)
            {
                double sine = amplitude * Math.Sin(Math.PI * 2 * frequency * time);
                time += 1.0 / 44100;
                short truncated = (short) Math.Round(sine * (Math.Pow(2, 15) - 1));
                buffer[i * 2] = (byte)(truncated & 0x00ff);
                buffer[i*2 + 1] = (byte) ((truncated & 0xff00) >> 8);

            }

            return count;

           }
    }
}
