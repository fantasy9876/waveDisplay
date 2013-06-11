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
                for (int i=0; i<data.Count; i++)
                {
                    float freq = i * (44000 / data.Count);
                    double logfreq = Math.Log(freq, 2) - Math.Log(440, 2);
                    var octave = Math.Floor(logfreq);
                    var note = logfreq - octave;

                    e.Graphics.DrawLine(Pens.Black, (float)(note * Width), (float)(500 + octave * 200), (float)(note * Width), (float)(500 + octave * 200 - data[i])); 
                }
        }
    }
}
