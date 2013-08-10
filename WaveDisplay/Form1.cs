using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NAudio.Dsp;

namespace WaveDisplay
{
    public partial class Form1 : Form
    {
        public static WaveIn waveIn = new WaveIn();
        public static WaveIn waveZoom=new WaveIn();
        public static int CurWavCount;
        public static int CurSpectrCount;
        public static int CurVerCount;
        public static string chosenFile = "";
        public static int[] chunkIndexRange=new int[2];
        public static bool mark=false, view=false;
        public static int[] markChunk = { 0, 0 };
        public const int stftChunkSize = 1024;
        Form3 octForm = new Form3();
        public static Bitmap bmpSpectro = new Bitmap(1, 1);

        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pictureBox1.MouseHover += new EventHandler(pictureBox1_MouseHover);
            pictureBox1.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
            pictureBox2.MouseWheel += new MouseEventHandler(pictureBox2_MouseWheel);
            pictureBox2.MouseHover += new EventHandler(pictureBox2_MouseHover);
            pictureBox2.MouseLeave += new EventHandler(pictureBox2_MouseLeave);
            
        }

        void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            this.ActiveControl = null;
        }

        void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            pictureBox1.Focus();
        }
     
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
           if (e.Delta > 0)
           {
               int tempCount = CurWavCount;
               int tempIdx1;
               if (CurWavCount > 20000)
                   CurWavCount -= 10000;
               else if (CurWavCount <= 20000 && CurWavCount > 1000)
                   CurWavCount -= 500;
               else if (CurWavCount <= 1000 && CurWavCount > 500)
                   CurWavCount -= 100;
               else
                   CurWavCount -= 50;
               Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
               if (CurWavCount > 0)
               {
                   tempIdx1 = levelScrollBar.Value;
                   if (waveIn.leftData.Count - tempIdx1 < CurWavCount)
                       tempIdx1 = waveIn.leftData.Count - CurWavCount;
                   waveZoom.leftData = waveIn.leftData.GetRange(tempIdx1, CurWavCount);
                   waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
                   levelScrollBar.Maximum = waveIn.leftData.Count - CurWavCount;
               }
               else
               {
                   CurWavCount = tempCount;
               }
           }
            else
           {
               int tempIdx2;

               if (CurWavCount < 500)
                   CurWavCount += 50;
               else if (CurWavCount >= 500 && CurWavCount < 1000)
                   CurWavCount += 100;
               else if (CurWavCount >= 1000 && CurWavCount < 20000)
                   CurWavCount += 500;
               else
                   CurWavCount += 10000;
               Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
               if (CurWavCount < waveIn.leftData.Count)
               {
                   tempIdx2 = levelScrollBar.Value;
                   if (waveIn.leftData.Count - tempIdx2 < CurWavCount)
                       tempIdx2 = waveIn.leftData.Count - CurWavCount;
                   waveZoom.leftData = waveIn.leftData.GetRange(tempIdx2, CurWavCount);
                   waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
                   levelScrollBar.Maximum = waveIn.leftData.Count - CurWavCount;
               }
               else
                   CurWavCount = waveIn.leftData.Count;              
           }   
        }

        void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            this.ActiveControl = null;
        }

        private void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            Bitmap bmpOut = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap bmpMark = new Bitmap(bmpOut);
            if (e.Delta > 0)
            {
                int tempCount = CurSpectrCount;
                int tempIdx1=levelScrollBar.Value;
                if (CurSpectrCount < 100)
                    CurSpectrCount -= 20;
                else
                    CurSpectrCount -= 100; ;
                if (CurSpectrCount > 0)
                {                    
                    if (waveIn.stftWav.Count - tempIdx1 < CurSpectrCount)
                        tempIdx1 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx1, CurSpectrCount);
                    bmpSpectro= waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
                    levelScrollBar.Maximum = waveIn.stftWav.Count - CurSpectrCount;                    
                }
                else
                {
                    CurSpectrCount = tempCount;
                }
                chunkIndexRange[0] = tempIdx1;
                chunkIndexRange[1] = tempIdx1 + CurSpectrCount-1;
            }
            else
            {
                int tempIdx2;
                tempIdx2 = levelScrollBar.Value;
                if (CurSpectrCount < 100)
                    CurSpectrCount += 20;
                else
                    CurSpectrCount += 100; ;
                if (CurSpectrCount < waveIn.stftWav.Count)
                {
                    if (waveIn.stftWav.Count - tempIdx2 < CurSpectrCount)
                        tempIdx2 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx2, CurSpectrCount);
                    bmpSpectro= waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
                    levelScrollBar.Maximum = waveIn.stftWav.Count - CurSpectrCount;
                }
                else
                {
                    CurSpectrCount = waveIn.stftWav.Count;
                    tempIdx2 = 0;
                }
                chunkIndexRange[0] = tempIdx2;
                chunkIndexRange[1] = tempIdx2 + CurSpectrCount-1;                                            
            }

            marksDisplay(ref bmpMark);
            using (Graphics G = Graphics.FromImage(bmpOut))
            {
                G.DrawImage(bmpSpectro, 0, 0);
                G.DrawImage(bmpMark, 0, 0);

            }
            pictureBox2.Image = bmpOut;
        }

        void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            pictureBox2.Focus();
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            OpenFD.FileName = "";
            OpenFD.Filter = "PCM wave File|*.wav";
            OpenFD.ShowDialog();
            chosenFile= OpenFD.FileName;
            if (chosenFile != "")
            {
                waveIn = new WaveIn();
                waveIn.waveExtract(chosenFile);
                waveIn.STFT(waveIn.leftData, stftChunkSize);
                waveIn.DrawAudio(waveIn.leftData, pictureBox1);    
                Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
                levelScrollBar.Maximum = 0;
            }
            
        }

        private void levelScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                waveZoom.leftData = waveIn.leftData.GetRange(levelScrollBar.Value, CurWavCount);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
                Console.WriteLine("value : " + levelScrollBar.Value.ToString());
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                waveZoom.stftWav = waveIn.stftWav.GetRange(levelScrollBar.Value, CurSpectrCount);
                chunkIndexRange[0] = levelScrollBar.Value;
                chunkIndexRange[1] = levelScrollBar.Value + CurSpectrCount - 1;
                Bitmap bmpOut = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Bitmap bmpMark = new Bitmap(bmpOut);
                bmpSpectro=waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
                marksDisplay(ref bmpMark);
                using (Graphics G = Graphics.FromImage(bmpOut))
                {
                    G.DrawImage(bmpSpectro, 0, 0);
                    G.DrawImage(bmpMark, 0, 0);
                }
                pictureBox2.Image = bmpOut;
            }           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex== 0)
            {
                if (chosenFile != "")
                {
                    waveIn.DrawAudio(waveIn.leftData, pictureBox1);
                    waveZoom.leftData = waveIn.leftData.ToList();
                    CurWavCount = waveZoom.leftData.Count;
                    Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
                    levelScrollBar.Maximum = 0;
                }
            }
            else if (e.TabPageIndex == 1)
            {
                if (chosenFile != "")
                {
                    if (view)
                        pictureBox2.Invalidate();
                    else
                    {
                        bmpSpectro = waveIn.spectrogram(waveIn.stftWav, pictureBox2);
                        pictureBox2.Image = bmpSpectro;
                        waveZoom.stftWav = waveIn.stftWav.ToList();
                        CurSpectrCount = waveZoom.stftWav.Count;
                        levelScrollBar.Maximum = 0;
                        chunkIndexRange[0] = 0;
                        chunkIndexRange[1] = CurSpectrCount;
                    }
                }
            }
        }

        private void markBut_Click(object sender, EventArgs e)
        {
            mark = (mark) ? false : true;
            markBut.Enabled = false;
        }

        private void viewOctBut_Click(object sender, EventArgs e)
        {
            bmpSpectro = waveIn.spectrogram(waveIn.stftWav, pictureBox2);
            
            chunkIndexRange[0] = 0;
            chunkIndexRange[1] = waveIn.stftWav.Count;
            Bitmap bmpOut = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap bmpMark = new Bitmap(bmpOut);
            mark = true;
            float[] markLocations= marksDisplay(ref bmpMark);
            mark = false;
            using (Graphics g = Graphics.FromImage(bmpMark))
            {
                RectangleF markRect = new RectangleF(markLocations[0], 0, markLocations[1] - markLocations[0], bmpMark.Height);
                g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.White)), markRect);
            }
            using (Graphics G = Graphics.FromImage(bmpOut))
            {
                G.DrawImage(bmpSpectro, 0, 0);
                G.DrawImage(bmpMark, 0, 0);
            }
            pictureBox2.Image = bmpOut;
            // get the waveData start and end index (data in time domain)
            // start index is the start point of the stft data chunk, end is the end point of stft data chunk

            int[] timeIndex = new int[2];
            timeIndex[0] = markChunk[0] * stftChunkSize / 4;
            timeIndex[1] = markChunk[1] * stftChunkSize / 4 + stftChunkSize;
            
            view = true;            
            List<short> octTimeData= waveIn.leftData.GetRange(timeIndex[0],(timeIndex[1]-timeIndex[0]+1));
                   
            //TODO: 
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
            List<float> octFFT = waveIn.FFT(octTimeData);
            float frate = (float)(waveIn.wavHeader.sampleRate) / (octFFT.Count * 2);
            octDisplay(octFFT, frate);
            octForm.Invalidate();
            octForm.Show();
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            markBut.Enabled = true;
            Bitmap bmpSave= new Bitmap(bmpSpectro);
            if (mark)
                bmpSave=new Bitmap(pictureBox2.Image);
            Bitmap bmpMark = new Bitmap (bmpSave);
            drawMarkLine(e.X,ref bmpMark);
            using (Graphics G = Graphics.FromImage(bmpSave))
            {
                G.DrawImage(bmpMark, 0, 0);
            }
            pictureBox2.Image = bmpSave;
            float chunkRate = (float)((chunkIndexRange[1] + 1 - chunkIndexRange[0]) /(float) bmpSave.Width);
            if (mark)
                markChunk[1] = (int)(chunkRate * e.X + chunkIndexRange[0]);
            else
                markChunk[0] = (int)(chunkRate * e.X + chunkIndexRange[0]);
        }

        public float[] marksDisplay(ref Bitmap bmpMark)
        {
            float pxRate = (float)((float)pictureBox2.Width / (chunkIndexRange[1] + 1 - chunkIndexRange[0]));
            float X1=0, X2=0;
            if (markChunk[0] >= chunkIndexRange[0] && markChunk[0] <= chunkIndexRange[1])
            {
                X1 = (float)(pxRate * (markChunk[0]-chunkIndexRange[0]));
                drawMarkLine(X1, ref bmpMark);
                if (mark && markChunk[1] >= chunkIndexRange[0] && markChunk[1] <= chunkIndexRange[1])
                {
                    X2 = (float)(pxRate * markChunk[1]);
                    drawMarkLine(X2, ref bmpMark);
                }
            }
            else
            {
                bmpMark.MakeTransparent();
                if (mark && markChunk[1] >= chunkIndexRange[0] && markChunk[1] <= chunkIndexRange[1])
                {
                    X2 = (float)(pxRate * markChunk[1]);
                    drawMarkLine(X2, ref bmpMark);
                }
            }
            return new float[2]{X1,X2};
        }

        public void drawMarkLine(float Xlocation, ref Bitmap Layer)
        {

            Layer.MakeTransparent();
            using (Graphics g = Graphics.FromImage(Layer))
            {
                Pen markPen = new Pen(Color.Green, 2.0f);
                markPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawLine(markPen, new PointF(Xlocation, 0), new PointF(Xlocation, (float)Layer.Height));
            }
        }

        public void octDisplay(List<float> inputData, float frate)
        {
            octForm.data = new List<float>(inputData);
            octForm.rate = frate;
          
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            bmpSpectro = waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
            Bitmap bmpOut = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap bmpMark = new Bitmap(bmpOut);
            marksDisplay(ref bmpMark);
            using (Graphics G = Graphics.FromImage(bmpOut))
            {
                G.DrawImage(bmpSpectro, 0, 0);
                G.DrawImage(bmpMark, 0, 0);

            }
            pictureBox2.Image = bmpOut;

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (levelScrollBar.Maximum == 0)
                waveIn.DrawAudio(waveIn.leftData, pictureBox1);
            else
            {
                waveZoom.leftData = waveIn.leftData.GetRange(levelScrollBar.Value, CurWavCount);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
            }
        }
    }

}
