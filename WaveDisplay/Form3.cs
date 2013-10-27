using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveDisplay
{
    public partial class Form3 : Form
    {
        public WaveIn wavedata=new WaveIn();
        public uint sampleRate;
        public List<string> NoteXML=new List<string>();
        public List<int> NoteList;
        public int stftChunkSize = 1024;
        public int IndexSelected=0;
        public bool isXML=false;

        public Form3()
        {
            InitializeComponent();
        }


        public List<float> fftGet(int noteIdx,ref float frate)
        {
            int[] timeIndex = new int[2];
            timeIndex[0] = NoteList[noteIdx] * stftChunkSize / 2;
            timeIndex[1] = NoteList[noteIdx + 1] * stftChunkSize / 2 + stftChunkSize;

            List<short> octTimeData = wavedata.leftData.GetRange(timeIndex[0], (timeIndex[1] - timeIndex[0] + 1));
            //Pad data to make it 2^n count 
            int npad0, n;
            n = (int)(Math.Log(octTimeData.Count) / Math.Log(2));
            if (Math.Pow(2, n) < octTimeData.Count)
            {
                npad0 = (int)(Math.Pow(2, n + 1) - octTimeData.Count);
                for (int i = 0; i < npad0; i++)
                {
                    octTimeData.Add(0);
                }
            }
            List<float> corrOuput = wavedata.autocorrelation(octTimeData,sampleRate); //Autocorrelation using inverse FFT   
            float corrMax = corrOuput.Max();
            for (int s = 0; s < corrOuput.Count; s++)
            {
                corrOuput[s] = corrOuput[s] / corrMax;
            }
            //output data directly to a chart, zoomable
            corrChart.Series.Clear();
            corrChart.Series.Add("corrSeries");
            corrChart.Series["corrSeries"].ChartType = SeriesChartType.FastLine;
            foreach (float item in corrOuput)
            {
                corrChart.Series["corrSeries"].Points.AddY((double)item);
            }
            List<float> octFFT = wavedata.FFT(octTimeData,false);
            frate = (float)(wavedata.wavHeader.sampleRate) / (octFFT.Count * 2);
            return octFFT;
        }
        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            float frate=0;
            List<float> data = fftGet(IndexSelected, ref frate);
            if (data != null)
            {
                float max = data.Max();
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    drawNoteName(g, pictureBox1);
                    for (int i = 0; i < data.Count; i++)
                    {
                        float freq = i * (44000 / (float)data.Count);
                        double logfreq = Math.Log(freq, 2) - Math.Log(440, 2) + 9.0 / 12;
                        var octave = Math.Floor(logfreq);
                        var note = logfreq - octave;
                      
                        if (octave >= -1 && octave <= 4)
                        {
                            var noteDraw = logfreq - octave;
                            var oct_base_pos = (pictureBox1.Height - 20) * (1 - (octave + 1) / 6);
                            g.DrawLine(Pens.Black, (float)(noteDraw * pictureBox1.Width), (float)oct_base_pos, (float)(noteDraw * pictureBox1.Width), (float)(oct_base_pos - data[i] * (pictureBox1.Height - 20) / (6 * max)));
                        }
                        if (isXML)
                           g.DrawString("Sheet Music Note: "+ NoteXML[IndexSelected], new Font("Arial", 14), new SolidBrush(Color.Brown), new Point(5,5));
                    }
                    pictureBox1.Image = bmp;
                }
            }
        }

        public void drawNoteName(Graphics G, PictureBox pic)
        {
            string[] noteSequence = new string[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
           
            for (int i = 0; i <= 5; i++)
            {
                var noteBase = (pic.Height - 20) * (float)((1 - (float)i/ 6));
                G.DrawLine(Pens.Black, 0, (float)noteBase, pic.Width, (float)noteBase);
                for (int j = 0; j <= 11; j++)
                {
                    G.DrawString(noteSequence[j]+(i+2).ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new PointF(j*pic.Width/12, noteBase));
                }
             }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            foreach (WaveIn.notePredict item in wavedata.notePredictList)
            {
                string noteView = item.NoteName + item.octave.ToString();
                noteListView.Items.Add(noteView);
            }
        }

        private void noteListView_ItemActivate(object sender, EventArgs e)
        {
           var indices= noteListView.SelectedIndices;
           IndexSelected = indices[0];
           Invalidate();
        }
    }
}
