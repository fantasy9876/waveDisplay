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
        public static MusicSheet musicSheet = new MusicSheet();
        public static int CurWavCount;
        public static int CurSpectrCount;
        public static int CurVerCount;
        public static string waveFile = "",XMLFile="";
        public static int XMLnoteIdx;
        public static int[] chunkIndexRange=new int[2];
        public static bool marked=false;
        public static int[] markChunk = { 0, 0 };
        public const int stftChunkSize = 1024;
        public static int frange;
        Form3 octForm = new Form3();
        public static Bitmap bmpSpectro = new Bitmap(1, 1);
        public static List<int> noteList= new List<int>(); // position of the STFT chunk 
        public feedbackViewer fbVisual = new feedbackViewer();
        public Form1()
        {
            InitializeComponent();
            pictureBox3.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pictureBox3.MouseHover += new EventHandler(pictureBox1_MouseHover);
            pictureBox3.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
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
            pictureBox3.Focus();
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
               if (CurWavCount > 0)
               {
                   tempIdx1 = levelScrollBar.Value;
                   if (waveIn.leftData.Count - tempIdx1 < CurWavCount)
                       tempIdx1 = waveIn.leftData.Count - CurWavCount;
                   waveZoom.leftData = waveIn.leftData.GetRange(tempIdx1, CurWavCount);
                   waveZoom.DrawAudio(waveZoom.leftData, pictureBox3);
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
           
               if (CurWavCount < waveIn.leftData.Count)
               {
                   tempIdx2 = levelScrollBar.Value;
                   if (waveIn.leftData.Count - tempIdx2 < CurWavCount)
                       tempIdx2 = waveIn.leftData.Count - CurWavCount;
                   waveZoom.leftData = waveIn.leftData.GetRange(tempIdx2, CurWavCount);
                   waveZoom.DrawAudio(waveZoom.leftData, pictureBox3);
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
            if (e.Delta > 0)
            {
                int tempCount = CurSpectrCount;
                int tempIdx1=levelScrollBar.Value;
                if (CurSpectrCount < 100)
                    CurSpectrCount -= 20;
                else
                    CurSpectrCount -= 500; ;
                if (CurSpectrCount > 0)
                {                    
                    if (waveIn.stftWav.Count - tempIdx1 < CurSpectrCount)
                        tempIdx1 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx1, CurSpectrCount);
                    bmpSpectro= waveZoom.spectrogram(pictureBox2,frange);
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
                    CurSpectrCount += 500; ;
                if (CurSpectrCount < waveIn.stftWav.Count)
                {
                    if (waveIn.stftWav.Count - tempIdx2 < CurSpectrCount)
                        tempIdx2 = waveIn.stftWav.Count - CurSpectrCount;
                    waveZoom.stftWav = waveIn.stftWav.GetRange(tempIdx2, CurSpectrCount);
                    bmpSpectro= waveZoom.spectrogram(pictureBox2,frange);
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
            Bitmap bmpOut = new Bitmap(bmpSpectro);
            List<float> X = new List<float>();
  
            using (Graphics G = Graphics.FromImage(bmpOut))
            {
               X= marksDisplay(ref bmpOut,true);

            }
            if (marked)
            {
                List<float> notePos = Pred_note_draw(X, ref bmpOut);
                if (XMLFile != "")
                    noteXML_draw(notePos, ref bmpOut);
            }
            pictureBox2.Image = bmpOut;

        }

        void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            pictureBox2.Focus();
        }

        private void levelScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                waveZoom.leftData = waveIn.leftData.GetRange(levelScrollBar.Value, CurWavCount);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox3);
            
            }
            else if (tabControl1.SelectedIndex == 1)
            { //get the current scrollbar value which is the current chunk value.
                waveZoom.stftWav = waveIn.stftWav.GetRange(levelScrollBar.Value, CurSpectrCount);
                List<float> X = new List<float>();
                chunkIndexRange[0] = levelScrollBar.Value;
                chunkIndexRange[1] = levelScrollBar.Value + CurSpectrCount - 1;                
                bmpSpectro=waveZoom.spectrogram(pictureBox2,frange);
                Bitmap bmpOut = new Bitmap(bmpSpectro);
                using (Graphics G = Graphics.FromImage(bmpOut))
                {
                   X=marksDisplay(ref bmpOut,true);
                }
                if (marked)
                {
                    List<float> notePos= Pred_note_draw(X, ref bmpOut);
                    if (XMLFile != "")
                        noteXML_draw(notePos, ref bmpOut);
                }
                pictureBox2.Image = bmpOut;
                waveZoom.spectroPow = waveIn.spectroPow.GetRange(levelScrollBar.Value, CurSpectrCount - 1);
                waveZoom.spectroDiffDraw( chart1);
               
            }
            else if (tabControl1.SelectedIndex == 0)
            {                
                Bitmap bmpOutput=new Bitmap(pictureBox1.Width,pictureBox1.Height);
                fbVisual.drawMusicNotation(ref bmpOutput,musicSheet, levelScrollBar.Value);

                List<WaveIn.notePredict> notePredictedPrint = new List<WaveIn.notePredict>(waveIn.notePredictList);
                int XMLnoteIdx = 0;
                while (string.Compare(musicSheet.NoteExtract[XMLnoteIdx].Name, "rest") == 0)
                {
                    WaveIn.notePredict notebuffer = new WaveIn.notePredict();
                    notebuffer.NoteName = "rest";
                    notebuffer.octave = -1;
                    notePredictedPrint.Insert(0, notebuffer);
                    XMLnoteIdx++;
                }
                
                if (notePredictedPrint.Count > levelScrollBar.Value)
                {
                    int Idx = levelScrollBar.Value;
                    int i = 0;
                    if (notePredictedPrint.Count-levelScrollBar.Value <= (bmpOutput.Width - 200) / 120)
                        while (i < notePredictedPrint.Count-levelScrollBar.Value)
                        {
                            fbVisual.drawActualNote(ref bmpOutput, notePredictedPrint[Idx], i);
                            Idx++;
                            i++;
                        }
                    else
                        while (i < (bmpOutput.Width - 200) / 120)
                        {
                            fbVisual.drawActualNote(ref bmpOutput, notePredictedPrint[Idx], i);
                            Idx++;
                            i++;
                        }
                }
                pictureBox1.Image = bmpOutput;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex== 2)
            {
                if (waveFile != "")
                {
                    waveIn.DrawAudio(waveIn.leftData, pictureBox3);
                    waveZoom.leftData = waveIn.leftData.ToList();
                    CurWavCount = waveZoom.leftData.Count;
                    levelScrollBar.Maximum = 0;
                }
            }
            else if (e.TabPageIndex == 1)
            {
                if (waveFile != "")
                {                  
                    waveZoom.stftWav = waveIn.stftWav.GetRange(0,1000);
                    bmpSpectro = waveZoom.spectrogram(pictureBox2,frange);
                    pictureBox2.Image = bmpSpectro;
                    waveZoom.spectroPow = waveIn.spectroPow.GetRange(levelScrollBar.Value, 200);
                    waveZoom.spectroDiffDraw(chart1);
                    CurSpectrCount = waveZoom.stftWav.Count;
                    levelScrollBar.Maximum = waveIn.stftWav.Count-CurSpectrCount;
                    chunkIndexRange[0] = 0;
                    chunkIndexRange[1] = CurSpectrCount;
                }
            }
            else if (e.TabPageIndex == 0)
            {
                Bitmap pic = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                levelScrollBar.Value = 0;
                levelScrollBar.Maximum = musicSheet.NoteExtract.Count- (pic.Width -200)/ 120;                
                fbVisual.drawMusicNotation(ref pic,musicSheet,0);

                List<WaveIn.notePredict> notePredictedPrint = new List<WaveIn.notePredict>(waveIn.notePredictList);
                int XMLnoteIdx = 0;
                while (string.Compare(musicSheet.NoteExtract[XMLnoteIdx].Name, "rest") == 0)
                {
                    WaveIn.notePredict notebuffer=new WaveIn.notePredict();
                    notebuffer.NoteName = "rest";
                    notebuffer.octave = -1;
                    notePredictedPrint.Insert(0, notebuffer);
                    XMLnoteIdx++;
                }
                int Idx = 0;
                if (notePredictedPrint.Count<=(pic.Width-200)/120)
                    while (Idx < notePredictedPrint.Count)
                    {
                        fbVisual.drawActualNote(ref pic, notePredictedPrint[Idx], Idx);
                        Idx++;
                    }
                else
                    while (Idx < (pic.Width - 200) / 120)
                    {
                        fbVisual.drawActualNote(ref pic, notePredictedPrint[Idx], Idx);
                        Idx++;
                    }
                pictureBox1.Image = pic;
                
            }
        }

        private void undoBut_Click(object sender, EventArgs e)
        {
            if (noteList.Count == 1)
                undoBut.Enabled = false;
            noteList.Remove(noteList.Last());
            if (waveIn.notePredictList.Count>0)
                waveIn.notePredictList.RemoveAt(waveIn.notePredictList.Count -1);
            Bitmap bmpOut = new Bitmap(bmpSpectro);
            if (noteList.Count > 0)
            {
                using (Graphics G = Graphics.FromImage(bmpOut))
                {
                    List<float> X = marksDisplay(ref bmpOut, true);
                    List<float> notePos = Pred_note_draw(X, ref bmpOut);
                    noteXML_draw(notePos, ref bmpOut);
                }
            }
            pictureBox2.Image = bmpOut;
           
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            marked = true;
            undoBut.Enabled = true;
            Bitmap bmpSave= new Bitmap(bmpSpectro);
       
            float chunkRate = (float)((chunkIndexRange[1] + 1 - chunkIndexRange[0]) / (float)bmpSave.Width);
            noteList.Add((int)(chunkRate * e.X + chunkIndexRange[0]));
            
            using (Graphics G = Graphics.FromImage(bmpSave))
            {

                if (noteList.Count != 1)
                {
                    int[] timeIndex = new int[2];
                    //only identify the latest 2 chunk index in note list since other notes prior have been identified
                    int chunkIdxTmp1 = noteList[noteList.Count - 2];
                    int chunkIdxTmp2 = noteList.Last();
                    //get time index of wavedata that corresponds to the chunk
                    timeIndex[0] = chunkIdxTmp1 * stftChunkSize / 2;
                    timeIndex[1] = chunkIdxTmp2 * stftChunkSize / 2 + stftChunkSize;
                    List<short> octTimeData = waveIn.leftData.GetRange(timeIndex[0], (timeIndex[1] - timeIndex[0] + 1));

                    //Pad data to make it 2^n count 
                    int npad0, n;
                    n = (int)(Math.Log(octTimeData.Count) / Math.Log(2));
                    if (Math.Pow(2, n) < octTimeData.Count)
                    {
                        npad0 = (int)(Math.Pow(2, n + 1) - octTimeData.Count);
                        for (int j = 0; j < npad0; j++)
                        {
                            octTimeData.Add(0);
                        }
                    }
                    List<float> corrOuput = waveIn.autocorrelation(octTimeData, waveIn.wavHeader.sampleRate); //Autocorrelation using inverse FFT 
                    float corrMax = corrOuput.Max();
                    for (int s = 0; s < corrOuput.Count; s++)
                    {
                        corrOuput[s] = corrOuput[s] / corrMax;
                        corrOuput[s] = (corrOuput[s] < 0.2) ? 0 : corrOuput[s];
                    }
                    waveIn.notePredictList.Add(waveIn.noteIdentify(corrOuput, waveIn.wavHeader.sampleRate,timeIndex[1]-timeIndex[0]+1));

                }
                List<float> X = marksDisplay(ref bmpSave, true);
                List<float> NotePos=Pred_note_draw(X, ref bmpSave);

                noteXML_draw(NotePos, ref  bmpSave);
                
            }
           
            pictureBox2.Image = bmpSave;

            
        }

        public List<float> marksDisplay(ref Bitmap bmpMark, bool display) // get all X-coefficient pixel of the mark lines then draw it.
        {
            float pxRate = (float)((float)pictureBox2.Width / (chunkIndexRange[1] + 1 - chunkIndexRange[0]));
            List<float> X = new List<float>();
            
            List<int> list2disp = noteList.FindAll(e => (e >= chunkIndexRange[0] && e <= chunkIndexRange[1]));
            for (int i = 0; i < list2disp.Count; i++)
            {
                float Xbuf = pxRate * (list2disp[i] - chunkIndexRange[0]);
                X.Add(Xbuf);
            }
            if (X.Count!=0 && display)
               drawMarkLine(X, ref bmpMark);
            
            return X;
        }

        public void drawMarkLine(List<float> Xlocation, ref Bitmap Layer)
        {
            using (Graphics g = Graphics.FromImage(Layer))
            {
                
                for (int i = 0; i < Xlocation.Count; i++)
                {
                    var penWidth=2.0f;
                    if (i != 0 && Xlocation[i] == Xlocation[i - 1])
                        penWidth = 3.0f;
                    Pen markPen = new Pen(Color.Red, penWidth);
                    markPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    g.DrawLine(markPen, new PointF(Xlocation[i], 0), new PointF(Xlocation[i], (float)Layer.Height));
                }
            }
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox2.Width >= 600 && pictureBox2.Height >= 400)
            {
                bmpSpectro = waveZoom.spectrogram(pictureBox2, frange);
                Bitmap bmpOut = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Bitmap bmpMark = new Bitmap(bmpOut);
                marksDisplay(ref bmpMark, true);
                using (Graphics G = Graphics.FromImage(bmpOut))
                {
                    G.DrawImage(bmpSpectro, 0, 0);
                    G.DrawImage(bmpMark, 0, 0);

                }
                pictureBox2.Image = bmpOut;
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width >= 600 && pictureBox1.Height >= 400)
            {
                Bitmap bmpOutput = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                fbVisual.drawMusicNotation(ref bmpOutput, musicSheet, levelScrollBar.Value);
                pictureBox1.Image = bmpOutput;
            }
        }

        public List<float> Pred_note_draw(List<float> X, ref Bitmap bmp)
        {
            List<float> notePos = new List<float>();
            List<string> notePick = new List<string>();
            List<int> indexList=new List<int>();
            int index=0;
           
            while(noteList[index]<chunkIndexRange[0] && index <noteList.Count)
            {
                index++;
            }

            while(noteList[index]<=chunkIndexRange[1]&& index!=noteList.Count-1)
            {
                notePick.Add(waveIn.notePredictList[index].NoteName+waveIn.notePredictList[index].octave.ToString());
                index++;
            }
            
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < X.Count-1; i++)
                {
                    float Xpos = X[i]-1+(X[i + 1] - X[i]) / 2;
                    g.DrawString(notePick[i], new Font("Arial", 10), new SolidBrush(Color.Yellow), new PointF(Xpos, 50));
                    notePos.Add(Xpos);
                }
            }
          
            return notePos;
        }

        private void audioFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //COMMENT OUT FOR DEBUG
            //OpenFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //OpenFD.FileName = "";
            //OpenFD.Filter = "PCM wave File|*.wav";
            //OpenFD.ShowDialog();
            //waveFile = OpenFD.FileName;
            ///////////////////

            //hardcode file name 
            waveFile = "C:\\Users\\TramN\\Documents\\BEB 801\\samples\\Jupiter.wav";
            ///////////////////
            if (waveFile != "")
            {
                waveIn = new WaveIn();
                waveIn.waveExtract(waveFile);
                waveIn.STFT(waveIn.leftData, stftChunkSize);
                //waveIn.DrawAudio(waveIn.leftData, pictureBox3);
                frange =(int) (10000 / (waveIn.wavHeader.sampleRate / stftChunkSize)); // use for examine only freq band under 10000k
                levelScrollBar.Maximum = 0;
                tabControl1.SelectedIndex = 1;
            }
        }

        private void musicXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //COMMENT OUT FOR DEBUG
            //OpenFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //OpenFD.FileName = "";
            //OpenFD.Filter = "Music XML File|*.xml";
            //OpenFD.ShowDialog();
            //XMLFile = OpenFD.FileName;
            XMLFile = "C:\\Users\\TramN\\Documents\\BEB 801\\samples\\Jupiter.xml";
            if (XMLFile != "" && marked==true)
            {
                musicSheet = new MusicSheet();
                musicSheet.musicXMLread(XMLFile);
                Bitmap bmp = new Bitmap(pictureBox2.Image);
                List<float> X = marksDisplay(ref bmp, false);
                List<float> NotePos = Pred_note_draw(X, ref bmp);
                noteXML_draw(NotePos, ref bmp);
                pictureBox2.Image = bmp;
            }
        }

        public void  noteXML_draw(List<float> location, ref Bitmap bmp)
        {

            int index = 0;
            int XMLnoteIdx = 0;
            while (noteList[index] < chunkIndexRange[0])
            {
                index++;
            }
            if (XMLFile != "" && marked)
            {
                while (string.Compare(musicSheet.NoteExtract[XMLnoteIdx].Name, "rest") == 0)
                {
                    XMLnoteIdx++;
                }
                XMLnoteIdx += index;
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        
                        string XMLnotePrint=musicSheet.NoteExtract[i + XMLnoteIdx].Name+ musicSheet.NoteExtract[i+XMLnoteIdx].octave.ToString();
                        if (musicSheet.NoteExtract[i + XMLnoteIdx].alter == 1)
                            XMLnotePrint += "#";
                        else if (musicSheet.NoteExtract[i + XMLnoteIdx].alter == -1)
                            XMLnotePrint += "b";
                        
                        g.DrawString(XMLnotePrint, new Font("Arial", 12), new SolidBrush(Color.Aqua), new PointF(location[i], 20));

                    }
                    Pen markPen = new Pen(Color.Aqua, 2.0f);
                    markPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    g.DrawLine(markPen, new PointF(0, 40), new PointF(bmp.Width, 45));                 
                }
            }
            
        }

        private void noteSpectr_but_Click(object sender, EventArgs e)
        {
            octForm = new Form3();
            octForm.wavedata.leftData = waveIn.leftData.ToList();
            octForm.wavedata = new WaveIn(waveIn);
            octForm.wavedata.notePredictList = waveIn.notePredictList.ToList();
            octForm.NoteList = noteList;
            octForm.sampleRate = waveIn.wavHeader.sampleRate;
            int index=0;
            if (XMLFile != "")
            {
                octForm.isXML = true;
                while (string.Compare(musicSheet.NoteExtract[index].Name, "rest") == 0)
                {
                    index++;
                }
                for (int i = index; i < musicSheet.NoteExtract.Count;i++ )
                {
                    octForm.NoteXML.Add(musicSheet.NoteExtract[i].Name);
                }
            }
            octForm.Show();

        }

        private void clear_Click(object sender, EventArgs e)
        {
            noteList.Clear();
            waveIn.notePredictList.Clear();
            pictureBox2.Image = bmpSpectro;
            marked = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                string point = e.X.ToString() + "," + e.Y.ToString();
                g.DrawString(point, new Font("Arial", 15), new SolidBrush(Color.Black), e.Location);
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox3_SizeChanged(object sender, EventArgs e)
        {
            if (levelScrollBar.Maximum == 0)
                waveIn.DrawAudio(waveIn.leftData, pictureBox3);
            else
            {
                waveZoom.leftData = waveIn.leftData.GetRange(levelScrollBar.Value, CurWavCount);
                waveZoom.DrawAudio(waveZoom.leftData, pictureBox3);
            }
        }

        private void Auto_but_Click(object sender, EventArgs e)
        {
            List<WaveIn.noteInterval> onSetNotes= waveIn.onSetDetect(stftChunkSize);
            noteList.Clear();
            foreach (WaveIn.noteInterval item in onSetNotes)
            {
                noteList.Add(item.startIdx);
                noteList.Add(item.endIdx);
            }
            Bitmap bmpSave=new Bitmap(bmpSpectro);
            marksDisplay(ref bmpSave, true);
            pictureBox2.Image = bmpSave;
        }
    }

}
