using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections;
using System.Diagnostics;

using MiddlewareControllerN;
using GameLogicN;

namespace PlayScreen
{
    /// <summary>
    /// Interaction logic for Play.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MiddlewareController middlewareController = new MiddlewareController();
        GameLogic gameLogic;
        private string value;
        private int stepCount; //stepId
        private Stopwatch time; //stepTime
        private int playerId; //playerId
        private string playerType; // playerType

        public Window1()
        {
            InitializeComponent();
            Label.Content = "Please load the minefield.xml before you start the game.";
            enableBtn(false);
            Play.IsEnabled = false;
            Computer.IsEnabled = false;
            LoadGame.IsEnabled = false;

            this.time = new Stopwatch();
            Random rand = new Random();
            this.playerId = rand.Next(1, 1000);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if(btn.Name == "Play") //start game
            {
                Label.Content = "Enjoy!!";
                enableBtn(true);
                Play.IsEnabled = false;
                LoadField.IsEnabled = false;
                LoadGame.IsEnabled = false;
                Computer.IsEnabled = true;
                this.time.Start();

            }
            else if(btn.Name == "LoadField") //load minefield.xml
            {
                Label.Content = "Play : New Game\nLoad Game : last game you played";
                this.middlewareController.CallLoadMinefieldXml();
                this.gameLogic = new GameLogic(middlewareController.mineList);

                Play.IsEnabled = true;
                LoadGame.IsEnabled = true;
                LoadField.IsEnabled = false;

                MessageBox.Show("minefield.xml has been loaded. Click the Play or Load Game!");
            }
            else if(btn.Name == "LoadGame") //load game.xml
            {
                Label.Content = "Enjoy!!";
                Play.IsEnabled = false;
                Computer.IsEnabled = true;

                this.middlewareController.CallLoadGameXml();
                this.gameLogic = new GameLogic(middlewareController.mineList);

                List<Hashtable> loadList = new List<Hashtable>();
                loadList = middlewareController.loadGameList;
               
                
                Hashtable loadHt = loadList[0];
                string playerId = loadHt["playerId"].ToString();
                this.playerId = Int32.Parse(playerId);
                List<Hashtable> infoList = (List<Hashtable>)loadHt["info"];

                this.stepCount = infoList.Count;
                enableBtn(true);
              
                foreach (Hashtable ht in infoList)
                {
                    string location = ht["location"].ToString();
                    string sign = ht["sign"].ToString();
                    if(sign == "M")
                    {
                        this.value = "LoadGameM";
                    }
                    settingLoadGame(location, sign);
                }
                if(infoList.Count == 9 - middlewareController.mineList.Count)
                {
                    this.value = "LoadGameW";
                }
                LoadGame.IsEnabled = false;
                Play.IsEnabled = false;

                MessageBox.Show("game.xml has been loaded.");
                gameStatus();
            }
            else if(btn.Name == "Computer") //choosing a field by computer
            {
                this.playerType = "computer";
                
                this.time.Start();

                string rowColumnContent = choosingByComputer();

                if(rowColumnContent != null)
                {
                    string[] rowColumnContentArr = new string[2];

                    rowColumnContentArr = rowColumnContent.Split(',');

                    this.stepCount++;
                    gameStatus();
                    this.time.Stop();

                    Hashtable ht = new Hashtable();
                    ht["stepId"] = stepCount;
                    ht["stepTime"] = time.ElapsedMilliseconds;
                    ht["playerId"] = this.playerId;
                    ht["playerType"] = this.playerType;
                    ht["playSign"] = rowColumnContentArr[1];
                    ht["rowColumn"] = rowColumnContentArr[0];

                    this.middlewareController.CallSaveGameXml(ht);
                }

                this.time.Reset();
                this.time.Start();

            }
            else
            {
                this.playerType = "user";

                btn.IsEnabled = false;
                string tag = btn.Tag.ToString();

                btn.Content = clickLocation(Int32.Parse(tag.Substring(0, 1)), Int32.Parse(tag.Substring(1, 1)));
                
                this.stepCount++;
                gameStatus();

                this.time.Stop();

                Hashtable ht = new Hashtable();
                ht["stepId"] = stepCount;
                ht["stepTime"] = time.ElapsedMilliseconds;
                ht["playerId"] = this.playerId;
                ht["playerType"] = this.playerType;
                ht["playSign"] = btn.Content;
                ht["rowColumn"] = tag;

                this.middlewareController.CallSaveGameXml(ht);

                this.time.Reset();
                this.time.Start();
 
            }
        }

        public void gameStatus()
        {
            if (value == "M")
            {
                Computer.IsEnabled = false;
                MessageBox.Show("Game Over");
                Label.Content = "Game Over. Game has been finished.";
                end(); 
                return;
            }
            
            if(value == "LoadGameM")
            {
                MessageBox.Show("Already Game Over");
                Label.Content = "Already Game Over. You can't play this game anymore.\nPlease start new game.";
                end();
                return;
            }
           
            if(value == "LoadGameW")
            {
                Computer.IsEnabled = false;
                MessageBox.Show("Already Win");
                Label.Content = "Already You Won.\nPlease start new game.";
                end();
                return;
            }

            if (this.stepCount == 9 - middlewareController.mineList.Count)
            {
                Computer.IsEnabled = false;
                MessageBox.Show("Win");
                Label.Content = "Win. Game has been finished.\nPlease start new game.";
                end();
                return;
            }
        }

        public string clickLocation(int row, int column)
        {
            gameLogic.getClickLocation(row, column);
            this.value = gameLogic.logic();
            
            return value;
        }

        public string choosingByComputer()
        {
            Random rand = new Random();
            int row = rand.Next(0, 3);
            int column = rand.Next(0, 3);       

            string locationByComputer = row + "" + column;

            string rowColumnContent = "";


            if ((ZeroZero.Tag.ToString() == locationByComputer) && (ZeroZero.IsEnabled == true))
            {
                ZeroZero.Content = clickLocation(row, column); 
                ZeroZero.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + ZeroZero.Content;
            }
            else if ((ZeroOne.Tag.ToString() == locationByComputer) && (ZeroOne.IsEnabled == true))
            {
                ZeroOne.Content = clickLocation(row, column);
                ZeroOne.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + ZeroOne.Content;
            }
            else if ((ZeroTwo.Tag.ToString() == locationByComputer) && (ZeroTwo.IsEnabled == true))
            {
                ZeroTwo.Content = clickLocation(row, column); 
                ZeroTwo.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + ZeroTwo.Content;
            }
            else if ((OneZero.Tag.ToString() == locationByComputer) && (OneZero.IsEnabled == true))
            {
                OneZero.Content = clickLocation(row, column); 
                OneZero.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + OneZero.Content;
            }
            else if ((OneOne.Tag.ToString() == locationByComputer) && (OneOne.IsEnabled == true))
            {
                OneOne.Content = clickLocation(row, column); 
                OneOne.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + OneOne.Content;
            }
            else if ((OneTwo.Tag.ToString() == locationByComputer) && (OneTwo.IsEnabled == true))
            {
                OneTwo.Content = clickLocation(row, column); 
                OneTwo.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + OneTwo.Content;
            }
            else if ((TwoZero.Tag.ToString() == locationByComputer) && (TwoZero.IsEnabled == true))
            {
                TwoZero.Content = clickLocation(row, column); 
                TwoZero.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + TwoZero.Content;
            }
            else if ((TwoOne.Tag.ToString() == locationByComputer) && (TwoOne.IsEnabled == true))
            {
                TwoOne.Content = clickLocation(row, column); 
                TwoOne.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + TwoOne.Content;
            }
            else if ((TwoTwo.Tag.ToString() == locationByComputer) && (TwoTwo.IsEnabled == true))
            {
                TwoTwo.Content = clickLocation(row, column); 
                TwoTwo.IsEnabled = false;
                return rowColumnContent = locationByComputer + "," + TwoTwo.Content;
            }
            else
            {
                MessageBox.Show(row + "," + column + " is already clicked by user or computer");
                return null;
            } 
        }

        public void settingLoadGame(string location, string sign)
        {
            
            if (location == ZeroZero.Tag.ToString())
            {
                ZeroZero.Content = sign;
                ZeroZero.IsEnabled = false;
            }
            else if (location == ZeroOne.Tag.ToString())
            {
                ZeroOne.Content = sign;
                ZeroOne.IsEnabled = false;
            }
            else if (location == ZeroTwo.Tag.ToString())
            {
                ZeroTwo.Content = sign;
                ZeroTwo.IsEnabled = false;
            }
            else if (location == OneZero.Tag.ToString())
            {
                OneZero.Content = sign;
                OneZero.IsEnabled = false;
            }
            else if (location == OneOne.Tag.ToString())
            {
                OneOne.Content = sign;
                OneOne.IsEnabled = false;
            }
            else if (location == OneTwo.Tag.ToString())
            {
                OneTwo.Content = sign;
                OneTwo.IsEnabled = false;
            }
            else if (location == TwoZero.Tag.ToString())
            {
                TwoZero.Content = sign;
                TwoZero.IsEnabled = false;
            }
            else if (location == TwoOne.Tag.ToString())
            {
                TwoOne.Content = sign;
                TwoOne.IsEnabled = false;
            }
            else if (location == TwoTwo.Tag.ToString())
            {
                TwoTwo.Content = sign;
                TwoTwo.IsEnabled = false;
            }
        }

        public void end()
        {
            ZeroZero.Content = clickLocation(0, 0);
            ZeroOne.Content = clickLocation(0, 1);            
            ZeroTwo.Content = clickLocation(0, 2);
            
            OneZero.Content = clickLocation(1, 0);        
            OneOne.Content = clickLocation(1, 1);           
            OneTwo.Content = clickLocation(1, 2);           

            TwoZero.Content = clickLocation(2, 0);          
            TwoOne.Content = clickLocation(2, 1);           
            TwoTwo.Content = clickLocation(2, 2);

            enableBtn(false);
        }

        public void enableBtn(bool value)
        {
            ZeroZero.IsEnabled = value;
            ZeroOne.IsEnabled = value;
            ZeroTwo.IsEnabled = value;
            OneZero.IsEnabled = value;
            OneOne.IsEnabled = value;
            OneTwo.IsEnabled = value;
            TwoZero.IsEnabled = value;
            TwoOne.IsEnabled = value;
            TwoTwo.IsEnabled = value;
        }
    }
}
