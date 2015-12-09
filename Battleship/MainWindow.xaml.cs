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

        private Setup setup;
        private ShipPlacement shipPlacement;
        private PlayVSComp playVSComp;


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
            
            

            //Add event handler
            setup.play += new EventHandler(shipSetup);
        }

        /// <summary>
        /// Phase 2: shipPlacement 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shipSetup(object sender, EventArgs e)
        {
            //Close setup
            grid.Children.Clear();

            //Resize window
            this.MinWidth = 460;
            this.MinHeight = 530;

            //Initialize ship placement phase
            shipPlacement = new ShipPlacement();

            //Add ship placement grid
            grid.Children.Add(shipPlacement);

            shipPlacement.play += new EventHandler(playGame);
        }

        /// <summary>
        /// Phase 3: PlayVSComp 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playGame(object sender, EventArgs e)
        {
            //Close shipPlacement
            grid.Children.Clear();

            //Resize window
            this.MinWidth = 800;
            this.MinHeight = 470;

            //Initialize game
            playVSComp = new PlayVSComp(setup.difficulty, shipPlacement.playerGrid,shipPlacement.shipIndexArray);

            //Add grid
            grid.Children.Add(playVSComp);

        }
    }
}
