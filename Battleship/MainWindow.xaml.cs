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

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid = new Grid();

        Setup setup;
        ShipPlacement shipPlacement;
        PlayVSComp playVSComp;

        enum Difficulty { Simple, Intelligent };
        Difficulty difficulty;

        String name;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void InitializeGame()
        {
            //Initialize window
            Content = grid;

            //Initiate setup
            setup = new Setup();
            grid.Children.Add(setup);
            
            //Get difficulty
            if (rbtnIntelligent.IsChecked.Value)
            {
                difficulty = Difficulty.Intelligent;
            }
            else
            {
                difficulty = Difficulty.Simple;
            }

            //Add event handler
            setup.play += new EventHandler(shipSetup);
        }
        private void shipSetup(object sender, EventArgs e)
        {
            //Close setup
            setup.Visibility = Visibility.Collapsed;

            //Resize window
            this.MinWidth = 460;
            this.MinHeight = 470;

            //Initialize ship placement phase
            shipPlacement = new ShipPlacement();

            //Add ship placement grid
            grid.Children.Add(shipPlacement);
            shipPlacement.HorizontalAlignment = HorizontalAlignment.Left;
            shipPlacement.VerticalAlignment = VerticalAlignment.Top;

            //shipPlacement.play += new EventHandler(playGame);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
