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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private NAudio.Wave.WaveFileReader waveFile = null;
        private NAudio.Wave.DirectSoundOut output = null;
        private NAudio.Wave.BlockAlignReductionStream mp3Stream = null;

        
        public MainWindow()
        {
            InitializeComponent();
        }   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File or MP3 File (*.wav, *.mp3)|*.wav;*.mp3";
            if (open.ShowDialog() != true) return;

            Dispose();

            output = new DirectSoundOut();
            if (open.FileName.EndsWith(".mp3"))
            {

                WaveStream waveStream = new NAudio.Wave.WaveFormatConversionStream(new WaveFormat(), new Mp3FileReader(open.FileName));
                mp3Stream = new BlockAlignReductionStream(waveStream);
                output.Init(mp3Stream);
            } else if(open.FileName.EndsWith(".wav"))
            {
                waveFile = new NAudio.Wave.WaveFileReader(open.FileName);
                output.Init(new NAudio.Wave.WaveChannel32(waveFile));
            }
            
            FileNameBox.Text = open.FileName;
            output.Play();

            pauseButton.IsEnabled = true;



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Paused)
                {
                    output.Play();
                }
                else if (output.PlaybackState == PlaybackState.Playing)
                {
                    output.Pause();
                }

            }
        }


        public void Dispose()
        {
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    output.Stop();
                    output.Dispose();
                    output = null;
                }
                if (waveFile != null)
                {
                    waveFile.Dispose();
                    waveFile = null;
                }
            }
            
        }
    }
}
