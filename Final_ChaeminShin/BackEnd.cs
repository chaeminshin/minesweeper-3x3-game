using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace BackEndN
{
    class BackEnd
    {
        //save minefield.xml
        public void SaveMineFieldXML(List<Hashtable> list)
        {
            string url = @"../../minefield.xml";
            XDocument xmldoc = new XDocument(new XDeclaration("1.0", "UTF-8", null));
            XElement xroot = new XElement("Fields");
            xmldoc.Add(xroot);

            int[,] array = new int[3, 3];

            int row = 0;
            int column = 0;
            string YN = "";

            foreach (Hashtable ht in list)
            {
                row = (int)ht["row"];
                column = (int)ht["column"];
                array[row, column] = 1;
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if(array[i,j] == 1)
                    {
                        YN = "yes";
                    }
                    else
                    {
                        YN = "no";
                    }
                    XElement xe =
                                new XElement("Field", new XAttribute("row", i), new XAttribute("column", j),
                                new XElement("Mine", new XAttribute("active", YN)));
                    xroot.Add(xe);
                }
            }
            xmldoc.Save(url);
        }   

        //load minefield.xml
        public List<Hashtable> LoadMineFieldXML()
        {
            List<Hashtable> list = new List<Hashtable>();
            
            string url = @"../../minefield.xml";

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                XmlNodeList xmlNodeList = xml.SelectNodes("Fields/Field");
                foreach (XmlNode xn in xmlNodeList)
                {
                    string active = xn["Mine"].Attributes["active"].Value;
                    if (active == "yes")
                    {
                        int mineRow = Int32.Parse(xn.Attributes["row"].Value);
                        int mineColumn = Int32.Parse(xn.Attributes["column"].Value);

                        Hashtable ht = new Hashtable();

                        ht.Add("active", active);
                        ht.Add("row", mineRow);
                        ht.Add("column", mineColumn);
                        list.Add(ht);
                        //Exception.Text = active + "," + mineRow + ", " + mineColumn;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return list;
        }


        public void GameXML(Hashtable ht)
        {
            string strFile = @"../../game.xml";
            FileInfo fileInfo = new FileInfo(strFile);
            if (fileInfo.Exists)
            {

                UpdateGameXML(ht);
            }
            else
            {
                CreateGameXML(ht);
            }
        }
        public void CreateGameXML(Hashtable ht)
        {
            string url = @"../../game.xml";
            XDocument xmldoc = new XDocument(new XDeclaration("1.0", "UTF-8", null));
            XElement xroot = new XElement("Game");
            xmldoc.Add(xroot);
            XElement xsub = new XElement("Move");
            xroot.Add(xsub);
            
            XElement xe =
                        new XElement("Step", new XAttribute("id", ht["stepId"]), new XAttribute("time", ht["stepTime"]),
                        new XElement("Player", new XAttribute("id", ht["playerId"]), new XAttribute("type", ht["playerType"])),
                        new XElement("Play", new XAttribute("sign", ht["playSign"]), ht["rowColumn"]));
            xsub.Add(xe);
              
            xmldoc.Save(url);
        }

        public string UpdateGameXML(Hashtable ht)
        {
            string url = @"../../game.xml";

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                XmlNode xroot = xml.DocumentElement;
                XmlElement xmove = (XmlElement)xroot.FirstChild;

                XmlNode xstep = xml.CreateElement("Step");
                
                XmlAttribute xstepid = xml.CreateAttribute("id");
                xstepid.Value = ht["stepId"].ToString();
                xstep.Attributes.Append(xstepid);
                
                XmlAttribute xsteptime = xml.CreateAttribute("time");
                xsteptime.Value = ht["stepTime"].ToString();
                xstep.Attributes.Append(xsteptime);
                
                xmove.AppendChild(xstep);

                XmlNode xplayer = xml.CreateElement("Player");
                XmlAttribute xplayerid = xml.CreateAttribute("id");
                xplayerid.Value = ht["playerId"].ToString();
                xplayer.Attributes.Append(xplayerid);
                XmlAttribute xplayertype = xml.CreateAttribute("type");
                xplayertype.Value = ht["playerType"].ToString();
                xplayer.Attributes.Append(xplayertype);
                xstep.AppendChild(xplayer);

                XmlNode xplay = xml.CreateElement("Play");
                xplay.InnerText = ht["rowColumn"].ToString();
                XmlAttribute xplaysign = xml.CreateAttribute("sign");
                xplaysign.Value = ht["playSign"].ToString();
                xplay.Attributes.Append(xplaysign);
                xstep.AppendChild(xplay);

                xml.Save(url);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }
        //public string LoadGameXML()
        public List<Hashtable> LoadGameXML()
        {
            List<Hashtable> list = new List<Hashtable>();

            string url = @"../../game.xml";

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                XmlNodeList xmlNodeList = xml.SelectNodes("Game");
                string lastPlayerId="";
                foreach (XmlNode xn in xmlNodeList)
                {
                    lastPlayerId = xn["Move"].LastChild.FirstChild.Attributes["id"].Value;
                }

                
                List<Hashtable> infoList = new List<Hashtable>();
                XmlNodeList xmlNodeListForSearch = xml.SelectNodes("Game/Move/Step");
                foreach (XmlNode xn in xmlNodeListForSearch)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("playerId", lastPlayerId);
                    if (lastPlayerId == xn["Player"].Attributes["id"].Value)
                    {
                        string stepId = xn["Player"].ParentNode.Attributes["id"].Value;
                        string location = xn["Play"].InnerText;
                        string sign = xn["Play"].Attributes["sign"].Value;

                        Hashtable infoHt = new Hashtable();
                        infoHt.Add("stepId", stepId);
                        infoHt.Add("location", location);
                        infoHt.Add("sign", sign);

                        infoList.Add(infoHt);

                        ht.Add("info", infoList);

                        list.Add(ht);
                    }
                }

            }
            catch (Exception ex)
            {
               
            }
            return list;
        }
    }
}
