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
        public List<float> data;
        public float rate;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            if (data != null)
            {
                float max = data.Max();
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using(Graphics g= Graphics.FromImage(bmp))
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        float freq = i * (44000 / data.Count);
                        double logfreq = Math.Log(freq, 2) - Math.Log(440, 2);
                        var octave = Math.Floor(logfreq);
                        var note = logfreq - octave;

                        g.DrawLine(Pens.Black, (float)(note * pictureBox1.Width), (float)(pictureBox1.Height - 10 - Math.Abs(octave) * 100), (float)(note * pictureBox1.Width), (float)(pictureBox1.Height - 10 - Math.Abs(octave) * 100 - data[i] * pictureBox1.Height / (5 * max)));
                    }
                    pictureBox1.Image = bmp;
                }
            }
        }
    }
}
