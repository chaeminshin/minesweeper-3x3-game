using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections;

using MiddlewareControllerN;

namespace SetMinesScreen
{
    /// <summary>
    /// Interaction logic for SetMines.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public List<Hashtable> list = new List<Hashtable>();
        public Window1()
        {
            InitializeComponent();
        }

        private void SetMines(string mine, int row, int column)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Mine", mine);
            ht.Add("row", row);
            ht.Add("column", column);
            list.Add(ht);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(this.list.Count != 0)
            {
                MiddlewareController middlewareController = new MiddlewareController();
                middlewareController.MinesForSave = this.list;
                middlewareController.CallSaveMinefieldXml();
                MessageBox.Show("successfully");
                SetLabel.Content = "Mines were created by user.";
            }
            else
            {
                MessageBox.Show("At least one mine must be set up.");
            }
        }
        
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            List<Hashtable> generateList = new List<Hashtable>();

            Random rand = new Random();
            int mineCount = rand.Next(1, 9);
            for(int i=0; i<=mineCount; i++)
            {
                Hashtable ht = new Hashtable();
                int row = rand.Next(0, 3);
                int column = rand.Next(0, 3);

                ht.Add("Mine", "mine");
                ht.Add("row", row);
                ht.Add("column", column);
                generateList.Add(ht);

            }

            MiddlewareController middlewareController = new MiddlewareController();
            middlewareController.MinesForSave = generateList;
            middlewareController.CallSaveMinefieldXml();
            MessageBox.Show("successfully");
            SetLabel.Content = "Mines were automatically created.\nMines are not displayed on this screen.";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            string tag = btn.Tag.ToString();

            btn.IsEnabled = false;
            btn.Content = "M";
            SetMines("mine", Int32.Parse(tag.Substring(0, 1)), Int32.Parse(tag.Substring(1, 1)));

        }

        private void PlayGame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PlayScreen.Window1 moveToPlayScreen = new PlayScreen.Window1();
            moveToPlayScreen.ShowDialog();
        }
    }
}
