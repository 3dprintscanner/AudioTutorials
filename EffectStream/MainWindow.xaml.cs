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
using NAudio.Dmo;
using NAudio.Wave;

namespace EffectStream
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlockAlignReductionStream stream = null;
        private DirectSoundOut output = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Wav File *.wave|*.wav";

            if (open.ShowDialog() != true) return;

            WaveChannel32 wave = new WaveChannel32(new WaveFileReader(open.FileName));

            EffectsStream effect = new EffectsStream(wave);

            stream = new BlockAlignReductionStream(effect);

            for (int i = 0; i < wave.WaveFormat.Channels; i++)
            {
                effect.Effects.Add(new EchoEffect());
            }

            output = new DirectSoundOut();

            output.Init(stream);
            output.Play();


        }
    }
}
