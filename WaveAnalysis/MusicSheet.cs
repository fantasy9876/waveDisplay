using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;

namespace WaveAnalysis
{
    public class MusicSheet
    {
        public char clefSign;//lef compose of clef sign in char
        public short clefPosition;//position of clef sign
        public short fifth;//fifth in circle of fiths
        public string fifthType;//1 for Major and 0 for Minor
        public short beat;
        public short beatType;
        public int division; //the total division of the music sheet in a quater note 

        public struct Note
        {
            public string Name; // name of the note or rest
            public short alter; // 1 for sharpness, -1 for flatness, 0 if none provided
            public short octave; // octave of the note
            public int duration; // duration of beat, in form of multiple of division
            public string type; //type of the note (quater, eight, whole, etc)
            public bool isDot; // does the note contain dot
            public string stemDirect; // direction of note stem up or down
            public string beam;// single, double,etc beam connetor with other notes
        }
        public List<Note> NoteExtract=new List<Note>();

        public MusicSheet()
        {
            NoteExtract.Clear();
        }

        public int musicXMLread(string filename)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(new FileStream(filename, FileMode.Open, FileAccess.Read));
            XmlNodeList measureNode = doc.SelectNodes("/score-partwise/part/measure");
            foreach (XmlNode node in measureNode)
            {
                foreach(XmlNode childNode in node.ChildNodes)
                {
                    switch(childNode.Name)
                    {
                        case "attributes": //assume attributes has all childnodes as below
                            division = Convert.ToInt32(childNode.SelectSingleNode("divisions").InnerText);

                            //get the key of the music
                            XmlNode selectKey = childNode.SelectSingleNode("key");
                            fifth = Convert.ToInt16(selectKey.SelectSingleNode("fifths").InnerText);
                            fifthType = selectKey.SelectSingleNode("mode").InnerText;

                            //get the time signature of the music
                            XmlNode timeSigna = childNode.SelectSingleNode("time");
                            beat = Convert.ToInt16(timeSigna.SelectSingleNode("beats").InnerText);
                            beatType = Convert.ToInt16(timeSigna.SelectSingleNode("beat-type").InnerText);

                            //get the clef of the music
                            XmlNode clefInfo = childNode.SelectSingleNode("clef");
                            clefSign = Convert.ToChar(clefInfo.SelectSingleNode("sign").InnerText);
                            clefPosition = Convert.ToInt16(clefInfo.SelectSingleNode("line").InnerText);
                            break;

                        case "note":
                            var aNote = new Note();
                            switch (childNode.FirstChild.Name)
                            {
                                case "rest":
                                    aNote.Name="rest";
                                    aNote.octave=-1;
                                    aNote.duration=Convert.ToInt32(childNode.SelectSingleNode("duration").InnerText);
                                    NoteExtract.Add(aNote);
                                    break;
                                case "pitch":
                                    //get the pitch name and duration 
                                    aNote.Name = childNode.FirstChild.SelectSingleNode("step").InnerText;
                                    var alter = childNode.FirstChild.SelectSingleNode("alter");
                                    if (alter!=null)
                                        aNote.alter = Convert.ToInt16(alter.InnerText);
                                    aNote.octave = Convert.ToInt16(childNode.FirstChild.SelectSingleNode("octave").InnerText);
                                    aNote.duration = Convert.ToInt32(childNode.SelectSingleNode("duration").InnerText);
                                    aNote.type = childNode.SelectSingleNode("type").InnerText;

                                    var stem = childNode.SelectSingleNode("stem");
                                    aNote.stemDirect =(stem!= null) ?stem.InnerText:"";

                                    var beam = childNode.SelectSingleNode("beam");
                                    aNote.beam = (beam != null)? beam.InnerText: "";

                                    var dot = childNode.SelectSingleNode("dot");
                                    aNote.isDot = (dot != null)? true: false;

                                    NoteExtract.Add(aNote);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }           
            }
            return 0;
        }
    }
}
