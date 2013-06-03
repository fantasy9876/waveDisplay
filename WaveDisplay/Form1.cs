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
        public static Bitmap bmpSpectro = new Bitmap(1, 1);
        public static int[] markChunk = { 0, 0 };
        public const int stftChunkSize = 1024;
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

        void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            pictureBox2.Focus();
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
                    CurSpectrCount -= 50; ;
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
                    CurSpectrCount += 50; ;
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

            //TODO: 
            

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
                Pen markPen = new Pen(Color.Green, 3.0f);
                markPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawLine(markPen, new PointF(Xlocation, 0), new PointF(Xlocation, (float)Layer.Height));
            }
        }
    }

    public struct waveHeader
    {
        public byte[] riffID;
        public byte[] fileFormat;
        public byte[] fmtID;
        public UInt16 audioFormat;
        public UInt16 nChannels;
        public uint sampleRate;
        public uint byteRate;
        public UInt16 blockAlign;
        public UInt16 bitSample;
        public byte[] dataID;
        public uint dataSize;
    }

    public class WaveIn
    {        
        public waveHeader wavHeader; 

        public List<short> leftData = new List<short>(); //Aussme if filewave is mono, use this leftData to extract data
        public List<short> rightData= new List<short>();
        public List<List<float>> stftWav = new List<List<float>>();
        public WaveIn(WaveIn previousWave)
        {
            leftData = previousWave.leftData;
            rightData = previousWave.rightData;
        }
        public WaveIn()
        {
            leftData.Clear();
            rightData.Clear();
            stftWav.Clear();
        }

        public void waveExtract(string filename)
        {
            byte[] tmpByte;
            try 
            {
                using (FileStream wave_fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var test = wave_fs.Length;
                    using (BinaryReader br = new BinaryReader(wave_fs))
                    {
                        wavHeader.riffID = br.ReadBytes(4);
                        tmpByte = br.ReadBytes(4);
                        wavHeader.fileFormat = br.ReadBytes(4);
                        wavHeader.fmtID = br.ReadBytes(4);
                        tmpByte = br.ReadBytes(4);
                        wavHeader.audioFormat = br.ReadUInt16();
                        wavHeader.nChannels = br.ReadUInt16();
                        wavHeader.sampleRate = br.ReadUInt32();
                        wavHeader.byteRate = br.ReadUInt32();
                        wavHeader.blockAlign = br.ReadUInt16();
                        wavHeader.bitSample = br.ReadUInt16();
                        wavHeader.dataID = br.ReadBytes(4);
                        wavHeader.dataSize = br.ReadUInt32();
            //assume the file read in has 16 bit per sample
                        for (int i = 0; i < (wave_fs.Length- 44)/ wavHeader.blockAlign; i++)
                        {
                            if (wavHeader.nChannels == 1)
                                leftData.Add(br.ReadInt16());
                            else if (wavHeader.nChannels == 2)
                            {
                                leftData.Add(br.ReadInt16());
                                rightData.Add(br.ReadInt16());
                            }
                        }

                    }
                    string riff = Encoding.UTF8.GetString(wavHeader.riffID);
                    string fmt = Encoding.UTF8.GetString(wavHeader.fmtID);
                    string format =Encoding.UTF8.GetString(wavHeader.fileFormat);
                    Console.WriteLine("wav File header " + riff + " " + format + " " + wavHeader.audioFormat.ToString());
                }
            }   
                     
            catch(FileNotFoundException e)
            {
                MessageBox.Show("cannot open the file " + e.FileName);
            }

        }

        public Bitmap DrawAudio(List<short> inputValues, PictureBox picDraw)
        {
            Bitmap bmp;
            int index = 0;
            int width = picDraw.Width;
            int height = picDraw.Height;
            SizeF sizef=new SizeF(); 
            RectangleF rectanglef=new RectangleF();
            bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                if (inputValues.Count >= 20000)
                {
                    List<short> inputNomalized = new List<short>();
                    inputNomalized = waveNormalize(inputValues, width, height);
                    for (int i = 0; i < (inputNomalized.Count) / 2 - 1; i++)
                    {
                        g.DrawLine(new Pen(Color.Blue), new Point(i, inputNomalized[index] + height / 2), new Point(i, inputNomalized[index + 1] + height / 2));
                        index += 2;
                    }
                    
                }
                else
                {
                    PointF previous = new PointF(0, height / 2);
                    for (int i = 0; i < inputValues.Count; i++)
                    {
                        float x = (float)(i * 1.0 * width / inputValues.Count);
                        float y = (float)height / 2 + inputValues[i] * height / (1 << 16);
                        PointF next = new PointF(x, y);
                        g.DrawLine(new Pen(Color.Green), previous, next);
                        if (inputValues.Count <= 500)
                        {
                            sizef = new SizeF(2, 2);
                            rectanglef = new RectangleF(previous, sizef);
                            g.FillRectangle(new SolidBrush(Color.Gray), rectanglef);
                        }
                        previous = next;                      
                    }
                }
                
            }
            picDraw.Image = bmp;
            return bmp;
        }

        public List<Int16> waveNormalize(List<short> inputValues, int pxRangeX, int pxRangeY)
        {
            int dataPerPixel = inputValues.Count / pxRangeX;
            List<short> tempValues = new List<short>();
            List<short> outputValues = new List<short>();
            int index = 0;
            for (int i = 0; i + dataPerPixel <= inputValues.Count; i += dataPerPixel)
            {
                tempValues = inputValues.GetRange(i, dataPerPixel);
                outputValues.Add(tempValues.Max());
                outputValues.Add(tempValues.Min());
                outputValues[index] = (short)((outputValues[index] * pxRangeY) / 65536);
                outputValues[index + 1] = (short)((outputValues[index + 1] * pxRangeY) / 65536);
                index += 2;
            }
            return outputValues;
        }

        public Bitmap spectrogram(List<List<float>> inputValues, PictureBox picdraw)
        {
            int NoFFt = inputValues[0].Count;
            int i, j;

            Bitmap bmp = new Bitmap(picdraw.Width, picdraw.Height);
            float maxData = inputValues.Max(column => column.Max());
            Console.WriteLine("maxData: "+ maxData.ToString());
            RectangleF rectF = new RectangleF();
            SizeF rectFsize = new SizeF();
            PointF coordF = new PointF();
            float Xscale = (float)picdraw.Width / inputValues.Count;
            float Yscale = (float)picdraw.Height / (NoFFt / 2);
            rectFsize = new SizeF(Xscale, Yscale);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                for (i = 0; i < inputValues.Count; i++)
                {      
                    for (j=0 ; j<NoFFt/2 ; j++)
                    {
                        coordF = new PointF(i* Xscale, j * Yscale);
                        rectF = new RectangleF(coordF, rectFsize);
                        g.FillRectangle(new SolidBrush(getColor(inputValues[i][(NoFFt / 2 - 1) - j], maxData)), rectF);
                    }
                }
            }
            return bmp;
        }

        public Color getColor(float input, float maxData)
        {
            
            float colorFactor = input / maxData;
            Color setColor1, setColor2;
            //Console.WriteLine("colorFactor: " + colorFactor.ToString());
            int newIntensity = 0;
            newIntensity = (int)(255 * colorFactor);
            if (newIntensity >100)
            {
                setColor1 = Color.FromArgb(255, newIntensity * 2 / 5, newIntensity, newIntensity);
                setColor2 = ControlPaint.Light(setColor1, 0.5f);
            }
            else if (newIntensity >= 40 && newIntensity <= 100)
            {
                setColor1 = Color.FromArgb(255, newIntensity * 2 / 5, newIntensity, newIntensity);
                setColor2 = ControlPaint.Light(setColor1, 0.4f);
            }
            else if (newIntensity >= 20 && newIntensity <= 40)
            {
                setColor1 = Color.FromArgb(255, newIntensity * 2 / 5, newIntensity, newIntensity);
                setColor2 = ControlPaint.Light(setColor1, 0.2f);
            }
            else
            {
                setColor1 = Color.FromArgb(255, newIntensity, newIntensity, newIntensity * 1 / 5);
                setColor2 = Color.FromArgb(255, setColor1);
            }
            return setColor2;

        }
        public void STFT(List<short> inputValues, short No)
        {
            int i, overLap = No / 4;
            List<float> fftChunk = new List<float>();
            int count= inputValues.Count;
            for (i = 0; i + No < count;i +=overLap)
            {
                fftChunk =FFT( inputValues.GetRange(i, No));

                stftWav.Add(fftChunk);
            }
            fftChunk = FFT(inputValues.GetRange(count - No, No));
            stftWav.Add(fftChunk);

        }
        public List<float> FFT(List<short> inputValues)
        {
            int N = inputValues.Count; //assume N =2^n
            int n = (int)(Math.Log(N) / Math.Log(2));
            int i;
            List<float> Real = inputValues.ConvertAll(y =>(float)y);
            List<float> Imagine = new List<float>();
            for (i = 0; i < inputValues.Count; i++)
            {
                Imagine.Add(0.0f);
            }
            //Naudio FFT test
            Complex[] Data = new Complex[Real.Count] ;
            for (int l = 0; l < Real.Count; l++)
            {
                Data[l].X = Real[l];
                Data[l].Y = Imagine[l];
            }

            NAudio.Dsp.FastFourierTransform.FFT(true, 10, Data);

            ////

            //float Re,Im;
            ////do bit reversal
            //int k,j = 0;
            //for (i = 0; i < N - 1; i++)
            //{
            //    if (i < j)
            //    {
            //        Re = Real[i];
            //        Im = Imagine[i];
            //        Real[i] = Real[j];
            //        Imagine[i] = Imagine[j];
            //        Real[j] = Re;
            //        Imagine[j] = Im;
            //    }
            //    k = N / 2;
            //    while (k <= j)
            //    {
            //        j -= k;
            //        k /= 2;
            //    }
            //    j += k;
            //}

            ////compute FFT
            //int m, b,x,a = 1;
            //float t1, t2;
            //float coef1=-1.0f;
            //float coef2=0.0f;
            //for (m = 0; m < n; m++)
            //{
            //    b = a;
            //    a *= 2;
            //    float c= 1.0f;//value of cos(2pi/N), N is large
            //              //so this value reach to 1
            //    float s=0.0f; //value of sin(2pi/N) reach to 0 when
            //              //N is large
            //    //loop as decimation in Time
            //    for (j = 0; j < b; j++)
            //    {
            //        for (i = j; i < n; i += a)
            //        {
            //            x = i + b;
            //            t1 = c * Real[x] - s * Imagine[x];
            //            t2 = c * Imagine[x] + s * Real[x];
            //            Real[x] = Real[i] - t1;
            //            Imagine[x] = Imagine[i] - t2;
            //            Real[i] += t1;
            //            Imagine[i] += t2;
            //        }
            //        c = c*coef1-s*coef2;
            //        s = c * coef2 + s * coef1;                    
            //    }
            //    coef2 = (float)-Math.Sqrt((1.0f - coef1) / 2.0f);
            //    coef1 = (float)Math.Sqrt((1.0f + coef1) / 2.0f);
            //}
            List<float> output=new List<float>();
            float outputMag;
            for (i = 0; i < N / 2; i++)
            {
                outputMag=(float)Math.Sqrt(Math.Pow(Data[i].X,2)+Math.Pow(Data[i].Y,2));
                output.Add(outputMag);
            }
            return output;
        }
    }

}
