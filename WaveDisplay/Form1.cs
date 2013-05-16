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
        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pictureBox1.MouseHover += new EventHandler(pictureBox1_MouseHover);
            pictureBox1.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
            
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
               //zoomIn.PerformClick();
               zoomIn_Click(sender, e); // faster than the method above
           }
            else
           {
               //zoomOut.PerformClick();
               zoomOut_Click(sender, e); //faster than the method above
           }   
        }
        public static WaveIn waveIn = new WaveIn();
        public static WaveIn waveZoom;
        public static int currentLevel;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            OpenFD.FileName = "";
            OpenFD.Filter = "PCM wave File|*.wav";
            OpenFD.ShowDialog();
            string chosenFile = "";
            chosenFile= OpenFD.FileName;
            waveIn.waveExtract(chosenFile);
            waveIn.DrawAudio(waveIn.leftData, pictureBox1);
            waveZoom = new WaveIn(waveIn);
            currentLevel = waveZoom.leftData.Count;
            Console.WriteLine("currentLevel : " + currentLevel.ToString());
            levelScrollBar.Maximum = 0;

        }

       

        private void zoomIn_Click(object sender, EventArgs e)
        {
            int tempLevel = currentLevel;
            int startLevel;
            if (currentLevel > 20000)
                currentLevel -= 10000;
            else if (currentLevel <= 20000 && currentLevel > 1000)
                currentLevel -= 500;
            else if (currentLevel <= 1000 && currentLevel > 500)
                currentLevel -= 100;
            else
                currentLevel -= 50;
            Console.WriteLine("currentLevel : " + currentLevel.ToString());           
            if (currentLevel > 0 )
            {
                startLevel = levelScrollBar.Value;
                if (waveIn.leftData.Count - startLevel < currentLevel)
                    startLevel = waveIn.leftData.Count - currentLevel;
                waveZoom.leftData = waveIn.leftData.GetRange(startLevel, currentLevel);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
                levelScrollBar.Maximum = waveIn.leftData.Count -currentLevel;
            }
            else
            {
                currentLevel = tempLevel;
            }
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            int startLevel;

            if (currentLevel < 500)
                currentLevel += 50;
            else if (currentLevel >= 500 && currentLevel < 1000)
                currentLevel += 100;
            else if (currentLevel >= 1000 && currentLevel < 20000)
                currentLevel += 500;
            else
                currentLevel += 10000;
            Console.WriteLine("currentLevel : " + currentLevel.ToString());
            if (currentLevel < waveIn.leftData.Count)
            {
                startLevel = levelScrollBar.Value;
                if (waveIn.leftData.Count - startLevel < currentLevel)
                    startLevel = waveIn.leftData.Count - currentLevel;
                waveZoom.leftData = waveIn.leftData.GetRange(startLevel, currentLevel);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
                levelScrollBar.Maximum = waveIn.leftData.Count - currentLevel;
            }
            else
            {
                currentLevel = waveIn.leftData.Count;
            }
               
            
        }

        private void levelScrollBar_Scroll(object sender, ScrollEventArgs e)
        {         
            waveZoom.leftData = waveIn.leftData.GetRange(levelScrollBar.Value, currentLevel);
            waveZoom.DrawAudio(waveZoom.leftData, pictureBox1);
            Console.WriteLine("value : " + levelScrollBar.Value.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
            SizeF sizef=new SizeF(); RectangleF rectanglef=new RectangleF();
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
        public List<List<float>> STFT(List<short> inputValues, short No)
        {
            int i, overLap = No / 2;
            List<List<float>> stftChunks = new List<List<float>>();
            List<float> fftChunk = new List<float>();
            int count= inputValues.Count;
            for (i = 0; i + No < count;i +=overLap)
            {
                fftChunk =FFT( inputValues.GetRange(i, No));
                stftChunks.Add(fftChunk);
            }
            fftChunk = FFT(inputValues.GetRange(count - No, No));
            stftChunks.Add(fftChunk);
            return stftChunks;

        }
        public List<float> FFT(List<short> inputValues)
        {
            int N = inputValues.Count; //assume N =2^n
            int n = (int)(Math.Log(N) / Math.Log(2));
            int i;
            List<float> Real = inputValues.Cast<float>().ToList();
            List<float> Imagine = new List<float>();
            for(i=0;i<Imagine.Count;i++)
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
