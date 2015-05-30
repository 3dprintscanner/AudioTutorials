using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NAudio.Wave;

namespace WaveFormPlotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File *.wav|*.wav";

            if (open.ShowDialog() != DialogResult.OK) return;

            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";

            WaveChannel32 waveChannel32 = new WaveChannel32(new WaveFileReader(open.FileName));

            byte[] buffer = new byte[16384];

            int read = 0;

            while (waveChannel32.Position < waveChannel32.Length)
            {

                read = waveChannel32.Read(buffer, 0, 16384);

                for (int i = 0; i < read / 4; i++)
                {
                    chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i*4));
                }
            }


        }
    }
}
