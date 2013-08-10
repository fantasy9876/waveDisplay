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
                using (Graphics g=Graphics.FromImage(bmp))
                {
                    drawNoteName(g, pictureBox1);
                    for (int i = 0; i < data.Count; i++)
                    {
                        float freq = i * (44000/ (float)data.Count);
                        double logfreq = Math.Log(freq, 2) - Math.Log(440, 2) + 9.0 / 12;
                        var octave = Math.Floor(logfreq);
                        if (octave >= -1 && octave <= 4)
                        {
                            var note = logfreq - octave;
                            var oct_base_pos = (pictureBox1.Height - 20) * (1 - (octave + 1) / 6);
                            g.DrawLine(Pens.Black, (float)(note * pictureBox1.Width), (float)oct_base_pos, (float)(note * pictureBox1.Width), (float)(oct_base_pos - data[i] * (pictureBox1.Height - 20) / (6 * max)));
                        }
                    }
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
            for (int i = 0; i <= 5; i++)
            {
                var noteBase = (pic.Height - 20) * (float)((1 - (float)i/ 6));
                G.DrawLine(Pens.Black, 0, (float)noteBase, pic.Width, (float)noteBase);
                for (int j = 0; j <= 11; j++)
                {
                    G.DrawString(noteSequence[j]+(i+3).ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new PointF(j*pic.Width/12, noteBase));
                }
            }
        }
    }
}
