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
    public class WaveIn
    {
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
        public waveHeader wavHeader;

        public List<short> leftData = new List<short>(); //Aussme if filewave is mono, use this leftData to extract data
        public List<short> rightData = new List<short>();
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
                        for (int i = 0; i < (wave_fs.Length - 44) / wavHeader.blockAlign; i++)
                        {
                            if (wavHeader.nChannels == 1)
                            {
                                if (wavHeader.bitSample == 8)
                                    leftData.Add((short)((br.ReadByte()-128)*65536/256));
                                else
                                    leftData.Add(br.ReadInt16());
                            }
                            else if (wavHeader.nChannels == 2)
                            {
                                if (wavHeader.bitSample == 8)
                                {
                                    leftData.Add((short)(br.ReadByte()));
                                    rightData.Add((short)(br.ReadByte()));
                                }
                                else
                                {
                                    leftData.Add(br.ReadInt16());
                                    rightData.Add(br.ReadInt16());
                                }
                            }
                        }

                    }
                    string riff = Encoding.UTF8.GetString(wavHeader.riffID);
                    string fmt = Encoding.UTF8.GetString(wavHeader.fmtID);
                    string format = Encoding.UTF8.GetString(wavHeader.fileFormat);
                    Console.WriteLine("wav File header " + riff + " " + format + " " + wavHeader.audioFormat.ToString());
                }
            }

            catch (FileNotFoundException e)
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
            SizeF sizef = new SizeF();
            RectangleF rectanglef = new RectangleF();
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

        public List<Int16> waveNormalize(List<short> inputValues, int pxRangeX, int pxRangeY) //normalized data to fit in window
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

        public Bitmap spectrogram(List<List<float>> inputValues, PictureBox picdraw,int frange)
        {
            int NoFFt = inputValues[0].Count;
            int i, j;
            Bitmap bmp = new Bitmap(picdraw.Width, picdraw.Height);
            List<float> maxRange = new List<float>();
            float maxData = inputValues.Max(column => column.GetRange(0,frange).Max());
            maxData=(float)(Math.Log10(maxData));
            RectangleF rectF = new RectangleF();
            SizeF rectFsize = new SizeF();
            PointF coordF = new PointF();
            float Xscale = (float)picdraw.Width / inputValues.Count;
            float Yscale = (float)picdraw.Height / frange;
            rectFsize = new SizeF(Xscale, Yscale);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                for (i = 0; i < inputValues.Count; i++)
                {
                    for (j = 0; j < frange; j++)
                    {
                        coordF = new PointF(i * Xscale, j * Yscale);
                        rectF = new RectangleF(coordF, rectFsize);
                        float colorData = inputValues[i][(NoFFt / 2 - 1) - j];
                        colorData = (colorData > 1) ? (float)Math.Log10(colorData) : 0;
                        g.FillRectangle(new SolidBrush(getColor(colorData, maxData)), rectF);
                    }
                }
            }
            return bmp;
        }

        public Color getColor(float input, float maxData)
        {

            float colorFactor = input / maxData;
            Color setColor1;
            //Console.WriteLine("colorFactor: " + colorFactor.ToString());
            int newIntensity = 0;
            newIntensity = (int)(255 * colorFactor);
            if (newIntensity > 50)
            {
                setColor1 = Color.FromArgb(255, newIntensity, newIntensity, newIntensity);
                //setColor2 = ControlPaint.Light(setColor1, 0.8f);
            }

            else
            {
                setColor1 = Color.FromArgb(255, newIntensity , newIntensity , 0);
                //setColor2 = ControlPaint.Light(setColor1, 0.6f);
            }

            return setColor1;

        }
        public void STFT(List<short> inputValues, short No)
        {
            int i, overLap = No / 4;
            List<float> fftChunk = new List<float>();
            int count = inputValues.Count;
            for (i = 0; i + No < count; i += overLap)
            {
                fftChunk = FFT(inputValues.GetRange(i, No));

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
            List<float> Real = inputValues.ConvertAll(y => (float)y);
            List<float> Imagine = new List<float>();
            for (i = 0; i < inputValues.Count; i++)
            {
                Imagine.Add(0.0f);
            }
            //Naudio FFT test
            Complex[] Data = new Complex[Real.Count];
            for (int l = 0; l < Real.Count; l++)
            {
                Data[l].X = Real[l];
                Data[l].Y = Imagine[l];
            }

            NAudio.Dsp.FastFourierTransform.FFT(true, n, Data);

            List<float> output = new List<float>();
            float outputMag;
            for (i = 0; i < N / 2; i++)
            {
                outputMag = (float)Math.Sqrt(Math.Pow(Data[i].X, 2) + Math.Pow(Data[i].Y, 2));
                output.Add(outputMag);
            }
            return output;
        }

        public string noteIdentify(List<float> fftInput, int scale)
        {
            string[] noteSequence = new string[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            List<float> samples = new List<float>(600);
            for (int i = 0; i < 600; i++)
            {
                samples.Add(0);
            }
            List<float> means = new List<float>(12);
            for (int i = 0; i < 12; i++)
            {
                means.Add(0);
            }

            for (int i = 0; i < fftInput.Count; i++)
            {
                float freq = i * (44000 / (float)fftInput.Count);
                double logfreq = Math.Log(freq, 2) - Math.Log(440, 2) + 9.5 / 12;
                var octave = Math.Floor(logfreq);
                if (octave >= -1 && octave <= 4)
                {
                    var note = logfreq - octave;
                    int index = (int)(note * 600);
                    samples[index] += fftInput[i];
                }
            }
            for (int j = 0; j < 12; j++)
            {
                List<float> pack= samples.GetRange(j*50,50);
                means[j] = pack.Average();       
                pack.Clear();
            }
            int maxIndex = means.IndexOf(means.Max());
            return noteSequence[maxIndex];
        }
    }
}
