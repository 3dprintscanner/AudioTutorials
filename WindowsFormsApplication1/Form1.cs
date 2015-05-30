using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<NAudio.Wave.WaveInCapabilities> sources = new List<WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            listView1.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem(source.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                listView1.Items.Add(item);
            }
        }

        private NAudio.Wave.WaveIn sourceStream = null;
        private DirectSoundOut output = null;
        private NAudio.Wave.WaveFileWriter waveFileWriter;
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            int devicenumber = listView1.SelectedItems[0].Index;

            sourceStream = new WaveIn();
            sourceStream.DeviceNumber = devicenumber;
            sourceStream.WaveFormat = new WaveFormat();

            WaveInProvider waveProvider = new WaveInProvider(sourceStream);

            output = new DirectSoundOut();

            output.Init(waveProvider);
            sourceStream.StartRecording();
            output.Play();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (output != null)
            {
                
                output.Stop();
                output.Dispose();
                output = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;

            }
            if (waveFileWriter != null)
            {
                waveFileWriter.Dispose();
                waveFileWriter = null;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Wave File *.wav| *.wav";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            int devicenumber = listView1.SelectedItems[0].Index;

            sourceStream = new WaveIn();
            sourceStream.DeviceNumber = devicenumber;
            sourceStream.WaveFormat = new WaveFormat(44100,NAudio.Wave.WaveIn.GetCapabilities(devicenumber).Channels);
            waveFileWriter = new WaveFileWriter(saveFileDialog.FileName,sourceStream.WaveFormat);
            sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(sourceStream_dataAvailable);

            sourceStream.StartRecording();  


        }

        private void sourceStream_dataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFileWriter == null) return;

            waveFileWriter.Write(e.Buffer,0,e.BytesRecorded);
            waveFileWriter.Flush();
        }
    }
}
