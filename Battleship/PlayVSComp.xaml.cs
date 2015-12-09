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
    /// Interaction logic for PlayVSComp.xaml
    /// </summary>
    public partial class PlayVSComp : UserControl
    {
        public Difficulty difficulty;
        public Grid[] playerGrid;
        private int[] shipIndexArray;
        public Grid[] compGrid;

        public PlayVSComp(Difficulty difficulty, Grid[] playerGrid, int [] shipIndexArray)
        {
            InitializeComponent();

            this.difficulty = difficulty;
            initiateSetup(playerGrid);
            this.shipIndexArray = shipIndexArray;
            
        }

        /// <summary>
        /// Initial setup for grid
        /// </summary>
        /// <param name="userGrid"></param>
        private void initiateSetup(Grid [] userGrid)
        {
            //Set computer grid
            compGrid = new Grid[100];
            CompGrid.Children.CopyTo(compGrid, 0);

            //Set player grid
            playerGrid = new Grid[100];
            PlayerGrid.Children.CopyTo(playerGrid, 0);

            //Set ships
            for (int i = 0; i < 100; i++)
            {
                playerGrid[i].Background = userGrid[i].Background;
                playerGrid[i].Tag = userGrid[i].Tag;
            }
          
        }

        /// <summary>
        /// Attack event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMouseDown(object sender, MouseButtonEventArgs e)
        {
            char X = validateXCoordinate(txtBoxX.Text);
            char Y = validateYCoordinate(txtBoxY.Text);
        }

        private char validateXCoordinate(string X)
        {
            if (X.Length != 1)
            {
                return '-';
            }

            char XCoord = X[0];
            if (Char.IsLetter(XCoord))
            {
                return XCoord;
            }
            return '-';
        }
        private char validateYCoordinate(string Y)
        {
            if (Y.Length != 1)
            {
                return '-';
            }

            char YCoord = Y[0];
            if (Char.IsDigit(YCoord))
            {
                return YCoord;
            }
            return '-';
        }
    }
}
