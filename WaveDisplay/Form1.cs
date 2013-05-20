using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WaveDisplay
{
    public partial class Form1 : Form
    {
        public static WaveIn waveIn = new WaveIn();
        public static WaveIn waveZoom;
        public static int CurWavCount;
        public static int CurSpectrCount;
        public static int CurVerCount;
        public string chosenFile = "";

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
            if (e.Delta > 0)
            {
                int tempCount = CurSpectrCount;
                int tempIdx1;
                CurSpectrCount -= 10; ;
                if (CurSpectrCount > 0)
                {
                    tempIdx1 = levelScrollBar.Value;
                    if (waveIn.stftWav.Count - tempIdx1 < CurSpectrCount)
                        tempIdx1 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx1, CurSpectrCount);
                    waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
                    //levelScrollBar.Maximum = waveIn.stftWav.Count - CurSpectrCount;
                   
                }
                else
                    CurSpectrCount = tempCount;
            }
            else
            {
                int tempIdx2;
                CurSpectrCount += 10; ;
                if (CurSpectrCount < waveIn.stftWav.Count)
                {
                    tempIdx2 = levelScrollBar.Value;
                    if (waveIn.stftWav.Count - tempIdx2 < CurSpectrCount)
                        tempIdx2 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx2, CurSpectrCount);
                    waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
                    levelScrollBar.Maximum = waveIn.stftWav.Count - CurSpectrCount;
                    
                }
                else
                    CurSpectrCount = waveIn.stftWav.Count; 
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            OpenFD.FileName = "";
            OpenFD.Filter = "PCM wave File|*.wav";
            OpenFD.ShowDialog();
            chosenFile= OpenFD.FileName;
            waveIn.waveExtract(chosenFile);
            waveIn.STFT(waveIn.leftData, 2048);
            waveIn.DrawAudio(waveIn.leftData, pictureBox1);
            waveZoom = new WaveIn(waveIn);
            CurWavCount = waveZoom.leftData.Count;
            Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
            levelScrollBar.Maximum = 0;
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
                waveZoom.spectrogram(waveZoom.stftWav, pictureBox2);
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
                    waveZoom = new WaveIn(waveIn);
                    CurWavCount = waveZoom.leftData.Count;
                    Console.WriteLine("CurWavCount : " + CurWavCount.ToString());
                    levelScrollBar.Maximum = 0;
                }
            }
            else if (e.TabPageIndex == 1)
            {
                if (chosenFile != "")
                {
                    waveIn.spectrogram(waveIn.stftWav, pictureBox2);
                    waveZoom = new WaveIn(waveIn);
                    CurSpectrCount = waveZoom.stftWav.Count;
                    levelScrollBar.Maximum = 0;
                }
            }
        }

       

    }

    public class WaveIn
    {
        struct waveHeader
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
        }

        public void waveExtract(string filename)
        {
            byte[] tmpByte;
            waveHeader wavHeader;

            try 
            {
                using (FileStream wave_fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
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
                        for (int i = 0; i < wavHeader.dataSize / wavHeader.blockAlign; i++)
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
                    string format =Encoding.UTF8.GetString(wavHeader.fileFormat);
                    //MessageBox.Show("wav File header " + riff + " " + format + " " + wavHeader.audioFormat.ToString());
                }
            }            
            catch(FileNotFoundException e)
            {
                MessageBox.Show("cannot open the file " + e.FileName);
            }

        }

        public void DrawAudio(List<short> inputValues, PictureBox picDraw)
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

        public void spectrogram(List<List<float>> inputValues, PictureBox picdraw)
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
            picdraw.Image = bmp;        
        }

        public Color getColor(float input, float maxData)
        {
            
            float colorFactor = input / maxData;
            //Console.WriteLine("colorFactor: " + colorFactor.ToString());
            int red=0,green=0,blue=0;
            red = (int)(255 * Math.Pow(colorFactor, 0.2));
            green = (int)(255 * Math.Pow(colorFactor, 0.2));
            blue = (int)(255 * Math.Pow(colorFactor, 0.7));                      
            return Color.FromArgb(255, red, green, blue);

        }
        public void STFT(List<short> inputValues, short No)
        {
            int i, overLap = No / 2;
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
            for(i=0;i< inputValues.Count;i++)
            {
                Imagine.Add(0.0f);
            }
            float Re,Im;
            //do bit reversal
            int k,j = 0;
            for (i = 0; i < N - 1; i++)
            {
                if (i < j)
                {
                    Re = Real[i];
                    Im = Imagine[i];
                    Real[i] = Real[j];
                    Imagine[i] = Imagine[j];
                    Real[j] = Re;
                    Imagine[j] = Im;
                }
                k = N / 2;
                while (k <= j)
                {
                    j -= k;
                    k /= 2;
                }
                j += k;
            }

            //compute FFT
            int m, b,x,a = 1;
            float t1, t2;
            float coef1=-1.0f;
            float coef2=0.0f;
            for (m = 0; m < n; m++)
            {
                b = a;
                a *= 2;
                float c= 1.0f;//value of cos(2pi/N), N is large
                          //so this value reach to 1
                float s=0.0f; //value of sin(2pi/N) reach to 0 when
                          //N is large
                //loop as decimation in Time
                for (j = 0; j < b; j++)
                {
                    for (i = j; i < n; i += a)
                    {
                        x = i + b;
                        t1 = c * Real[x] - s * Imagine[x];
                        t2 = c * Imagine[x] + s * Real[x];
                        Real[x] = Real[i] - t1;
                        Imagine[x] = Imagine[i] - t2;
                        Real[i] += t1;
                        Imagine[i] += t2;
                    }
                    c = c*coef1-s*coef2;
                    s = c * coef2 + s * coef1;                    
                }
                coef2 = (float)-Math.Sqrt((1.0f - coef1) / 2.0f);
                coef1 = (float)Math.Sqrt((1.0f + coef1) / 2.0f);
            }
            List<float> output=new List<float>();
            float outputMag;
            for (i = 0; i < N / 2; i++)
            {
                outputMag=(float)Math.Sqrt(Math.Pow(Real[i],2)+Math.Pow(Imagine[i],2));
                output.Add(outputMag);
            }
            return output;
        }
    }

}
