using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WaveDisplay
{
    public partial class Form3 : Form
    {
        public WaveIn wavedata=new WaveIn();
        public List<string> NotePredict;
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
            timeIndex[0] = NoteList[noteIdx] * stftChunkSize / 4;
            timeIndex[1] = NoteList[noteIdx + 1] * stftChunkSize / 4 + stftChunkSize;

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
            List<float> octFFT = wavedata.FFT(octTimeData);
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
                float[] Ypos = new float[pictureBox1.Width];
                Array.Clear(Ypos, 0, Ypos.Length);
               
                for (int i = 0; i < data.Count; i++)
                {
                    float freq = i * (44000/ (float)data.Count);
                    double logfreq = Math.Log(freq, 2) - Math.Log(440, 2) + 9.5 / 12 ;
                    var octave = Math.Floor(logfreq);
                    if (octave >= -1 && octave <= 4)
                    {
                        var note = logfreq - octave;
                        int Xpos = (int)(note * pictureBox1.Width);
                        Ypos[Xpos] += data[i];
                    }
                }
                 using (Graphics g=Graphics.FromImage(bmp))
                {
                    drawNoteName(g, pictureBox1);
                    for(int i=0; i<pictureBox1.Width; i++)
                    {
                        g.DrawLine(Pens.Black, (float)(i), pictureBox1.Height - 30, (float)(i), (float)(pictureBox1.Height - 30 - Ypos[i] * (pictureBox1.Height - 30) / max));
                    }
                    if (isXML)
                        g.DrawString("Sheet Music Note: "+ NoteXML[IndexSelected], new Font("Arial", 14), new SolidBrush(Color.Brown), new Point(5,5));
                }
                pictureBox1.Image = bmp;
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel=true;
            this.Hide();
        }

        public void drawNoteName(Graphics G, PictureBox pic)
        {
            string[] noteSequence = new string[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
           
            G.DrawLine(Pens.Black, 0, pic.Height-30, pic.Width, pic.Height-30);
            for (int j = 0; j <= 11; j++)
            {
                G.DrawString(noteSequence[j], new Font("Arial", 10), new SolidBrush(Color.Black), new PointF((float)(j+0.5)*pic.Width/12, pic.Height-30));
            }            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            foreach (string item in NotePredict)
            {
                noteListView.Items.Add(item);
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
