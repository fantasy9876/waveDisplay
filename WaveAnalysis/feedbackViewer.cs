using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;

namespace WaveAnalysis
{
    public class feedbackViewer
    {
        public void drawMusicNotation(ref Bitmap pic, MusicSheet XMLmusicData,int startIdx, int space=120)
        {
            Font Musical = new Font("MusicalSymbols", 100, GraphicsUnit.Pixel);
            int staff_height = pic.Height/2 - 100;

            
            using (Graphics G= Graphics.FromImage(pic))
            {
                //Draw Music Staff
                G.DrawLine(new Pen(Color.Black, 3), new Point(16, staff_height + 39), new Point(16, staff_height + 140)); //draw beginning bar

                //staff bars are contructed as short peaces of staff bar character attached to each other 
                G.DrawString(char.ConvertFromUtf32(0x3D), Musical, new SolidBrush(Color.Black), new Point(0, staff_height)); 
                int i = 0; //offset of the music staff
                while (i < pic.Width)
                {
                    G.DrawString(char.ConvertFromUtf32(0x3D),Musical,new SolidBrush(Color.Black),new Point(i +103, staff_height));
                    i += 103;
                }
                G.DrawString(char.ConvertFromUtf32(0x26),Musical,new SolidBrush(Color.Black),new Point(0,staff_height));
                G.DrawString("Precision offset", new Font("Arial", 12), new SolidBrush(Color.Black), new PointF(16, staff_height - 50));
                G.DrawString("Duration (sec)", new Font("Arial", 12), new SolidBrush(Color.Black), new PointF(16, staff_height + 225));

                if (XMLmusicData.NoteExtract.Count !=0)
                {
                    List<MusicSheet.Note> NoteList = XMLmusicData.NoteExtract.GetRange(startIdx, (pic.Width - 200) / 120);
                    //Draw Note, assume default space = 120;
                    i = 0;
                    foreach (MusicSheet.Note noteItem in NoteList)
                    {
                        string notePrint = "";
                        string noteType;
                        if (noteItem.Name == "rest")
                            noteType = "rest";
                        else
                            noteType = noteItem.type;
                        notePrint = notePrintCode(noteType, noteItem.stemDirect, noteItem.duration, XMLmusicData.division);

                        //assume music clef is G and note G is on the 2nd line of music staff 
                        int shift = noteCharShift(noteItem.Name, noteItem.octave);

                        G.DrawString(notePrint, Musical, new SolidBrush(Color.Black), new PointF(200 + i * space, staff_height + shift));
                        if (noteItem.isDot)
                            G.DrawString(char.ConvertFromUtf32(0x2E), Musical, new SolidBrush(Color.Black), new PointF(245 + i * space, staff_height + shift));
                        if ((noteItem.Name == "B" && noteItem.octave == 3) || (noteItem.Name == "C" && noteItem.octave == 4))
                            G.DrawString(char.ConvertFromUtf32(0x5F), Musical, new SolidBrush(Color.Black), new PointF(195 + i * space, staff_height + 25));
                        if ((noteItem.Name == "B" && noteItem.octave == 5) || (noteItem.Name == "A" && NoteList[i].octave == 5))
                            G.DrawString(char.ConvertFromUtf32(0x5F), Musical, new SolidBrush(Color.Black), new PointF(190 + i * space, staff_height - 125));
                        i++;
                    }                
                }               
            }
        }
        public void drawActualNote(ref Bitmap pic,WaveIn.notePredict notePredicted, int offset, int space = 120)
        {
            //assume note string is in format "Name + octave+ shaft/flat"
            Font Musical = new Font("MusicalSymbols", 100, GraphicsUnit.Pixel);
            int staff_height = pic.Height / 2 - 100;
            string noteName=""; short octave=-1;

            string notePrint="";
            if (notePredicted.NoteName == "rest")
            {
                notePrint = char.ConvertFromUtf32(0xCE);
                noteName = "rest";
            }
            else if (notePredicted.NoteName == "NaN")
            {
                notePrint = char.ConvertFromUtf32(0x0020);
                noteName = "NaN"; 
            }
            else
            {
                notePrint = char.ConvertFromUtf32(0xF6);
                noteName = notePredicted.NoteName[0].ToString();
                octave = notePredicted.octave;
            }
            int shift = noteCharShift(noteName, octave);
            
            //draw note, percentage and duration
            using (Graphics G = Graphics.FromImage(pic))
            {
                G.DrawString(notePrint, Musical, new SolidBrush(Color.FromArgb(180, Color.Red)), new PointF(200 + offset * space, staff_height + shift));
                G.DrawString(notePredicted.percentage.ToString()+"%", new Font("Arial", 12), new SolidBrush(Color.Black), new PointF(200 + offset * space, staff_height - 50));
                G.DrawString(notePredicted.duration.ToString("0.00") , new Font("Arial", 12), new SolidBrush(Color.Black), new PointF(200 + offset * space, staff_height + 225));
            }


        }
        public string notePrintCode(string type, string stemDirect, int duration, int division)
        {
            string notePrint="";
            if (type == "rest")
            {
                float durationRatio = duration / division;
                if (durationRatio == 4)
                    notePrint = char.ConvertFromUtf32(0x2F);
                else if (durationRatio == 3)
                    notePrint = char.ConvertFromUtf32(0xE3) + char.ConvertFromUtf32(0xB7);
                else if (durationRatio == 2)
                    notePrint = char.ConvertFromUtf32(0xE3);
                else if (durationRatio == 1)
                    notePrint = char.ConvertFromUtf32(0xB7);
                else if (durationRatio == 0.5)
                    notePrint = char.ConvertFromUtf32(0xEE);
                else if (durationRatio == 0.25)
                    notePrint = char.ConvertFromUtf32(0xCE);
                else if (durationRatio == 0.125)
                    notePrint = char.ConvertFromUtf32(0xE4);
                else if (durationRatio == 0.0625)
                    notePrint = char.ConvertFromUtf32(0xC5);
                else if (durationRatio == 0.03125)
                    notePrint = char.ConvertFromUtf32(0xA8);
                else if (durationRatio == 0.015625)
                    notePrint = char.ConvertFromUtf32(0xF4);
                else if (durationRatio == 0.0078125)
                    notePrint = char.ConvertFromUtf32(0xE5);
            }
            else
            {
                switch (type)
                {
                    case ("long"):
                        notePrint = char.ConvertFromUtf32(0xDD);
                        break;
                    case ("breve"):
                        notePrint = char.ConvertFromUtf32(0x57);
                        break;
                    case ("whole"):
                        notePrint = char.ConvertFromUtf32(0x77);
                        break;
                    case ("half"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x48);
                        else
                            notePrint = char.ConvertFromUtf32(0x68);
                        break;
                    case ("quarter"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x51);
                        else
                            notePrint = char.ConvertFromUtf32(0x71);
                        break;
                    case ("eighth"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x45);
                        else
                            notePrint = char.ConvertFromUtf32(0x65);
                        break;
                    case ("16th"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x58);
                        else
                            notePrint = char.ConvertFromUtf32(0x78);
                        break;
                    case ("32nd"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x52);
                        else
                            notePrint = char.ConvertFromUtf32(0x72);
                        break;
                    case ("64th"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0xEF);
                        else
                            notePrint = char.ConvertFromUtf32(0xC6);
                        break;
                    case ("128th"):
                        if (stemDirect == "down")
                            notePrint = char.ConvertFromUtf32(0x82);
                        else
                            notePrint = char.ConvertFromUtf32(0x8D);
                        break;

                    default:
                        break;
                }
            }
            return notePrint;
        }
        public int noteCharShift(string name, short octave)
        {
            int shift = 0;
            //assume music clef is G and note G is on the 2nd line of music staff 
            switch (name)
            {
                case ("rest"):
                    shift = -50;
                    break;
                case ("B"):
                    if (octave == 3)
                        shift = 37;
                    else if (octave == 4)
                        shift = -50;
                    else if (octave == 5)
                        shift = -137;
                    break;
                case ("C"):
                    if (octave == 4)
                        shift = 25;
                    else if (octave == 5)
                        shift = -62;
                    break;
                case ("D"):
                    if (octave == 4)
                        shift = 12;
                    else if (octave == 5)
                        shift = -75;
                    break;
                case ("E"):
                    if (octave == 5)
                        shift = -87;
                    break;
                case ("F"):
                    if (octave == 4)
                        shift = -12;
                    else if (octave == 5)
                        shift = -100;
                    break;
                case ("G"):
                    if (octave == 4)
                        shift = -25;
                    else if (octave == 5)
                        shift = -112;
                    break;
                case ("A"):
                    if (octave == 4)
                        shift = -37;
                    else if (octave == 5)
                        shift = -125;
                    break;
                default:
                    break;
            }
            return shift;
        }

        public void noteMatching(ref List<WaveIn.notePredict> predictedNotes, List<MusicSheet.Note> XMLnotes)
        {
            int i = 0;
            while (i < predictedNotes.Count)
            {
                if (predictedNotes[i].NoteName == "NaN")
                    i++;
                else
                {
                    if (predictedNotes[i].NoteName[0] != XMLnotes[i].Name[0])
                    {
                        
                        if (predictedNotes[i].NoteName[0] == XMLnotes[i + 1].Name[0])
                        {                            
                            WaveIn.notePredict notebuffer = new WaveIn.notePredict();
                            notebuffer.NoteName = "NaN";
                            notebuffer.octave = -1;
                            predictedNotes.Insert(i, notebuffer);
                        }
                    }
                    i++;
                }
            }
        }
    }    
}
