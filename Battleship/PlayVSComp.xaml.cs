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
        int turnCount = 0;

        int pCarrierCount = 5, cCarrierCount = 5;
        int pBattleshipCount = 4, cBattleshipCount = 4;
        int pSubmarineCount = 3, cSubmarineCount = 3;
        int pCruiserCount = 3, cCruiserCount = 3;
        int pDestroyerCount = 2, cDestroyerCount = 2;

        public PlayVSComp(Difficulty difficulty, Grid[] playerGrid, int[] shipIndexArray)
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
        private void initiateSetup(Grid[] userGrid)
        {
            //Set computer grid
            compGrid = new Grid[100];
            CompGrid.Children.CopyTo(compGrid, 0);
            for (int i = 0; i < 100; i++)
            {
                compGrid[i].Tag = "water";
            }
            setupCompGrid();
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
        private void setupCompGrid()
        {
            Random random = new Random();
            int[] shipSizes = new int[] { 2, 3, 3, 4, 5 };
            string[] ships = new string[] { "destroyer","cruiser","submarine","battleship","carrier" };
            int size, index;
            string ship;
            Orientation orientation;
            bool unavailableIndex = true;

            for(int i = 0; i < shipSizes.Length; i++)
            {
                //Set size and ship type
                size = shipSizes[i];
                ship = ships[i];
                unavailableIndex = true;

                if (random.Next(0, 2) == 0)
                    orientation = Orientation.Horizontal;
                else
                    orientation = Orientation.Vertical;

                //Set ships
                if (orientation.Equals(Orientation.Horizontal))
                {
                    index = random.Next(0, 100);
                    while (unavailableIndex == true)
                    {
                        unavailableIndex = false;

                        while ((index + size - 1) % 10 < size - 1)
                        {
                            index = random.Next(0, 100);
                        }

                        for (int j = 0; j < size; j++)
                        {
                            if (index + j > 99 || !compGrid[index + j].Tag.Equals("water"))
                            {
                                index = random.Next(0,100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < size; j++)
                    {
                        compGrid[index + j].Tag = ship;
                        compGrid[index + j].Background = new SolidColorBrush(Colors.LightGreen); //remove after testing
                    }
                }
               else
                {
                    index = random.Next(0, 100);
                    while (unavailableIndex == true)
                    {
                        unavailableIndex = false;

                        while (index / 10 + size * 10 > 100)
                        {
                            index = random.Next(0, 100);
                        }

                        for (int j = 0; j < size * 10; j += 10)
                        {
                            if (index + j > 99 || !compGrid[index + j].Tag.Equals("water"))
                            {
                                index = random.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < size * 10; j += 10)
                    {
                        compGrid[index + j].Tag = ship;
                        compGrid[index + j].Background = new SolidColorBrush(Colors.LightGreen); //remove after testing
                    }
                }

            }


        }

        /// <summary>
        /// Attack event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Set sender to square chosen
            Grid square = (Grid)sender;

            //Check if player turn yet
            if (turnCount % 2 != 0)
            {
                return;
            }

            switch (square.Tag.ToString())
            {
                case "water":
                    square.Tag = "miss";
                    square.Background = new SolidColorBrush(Colors.Blue);
                    return;
                case "miss":
                case "hit":
                    return;
                case "destroyer":
                    cDestroyerCount--;
                    break;
                case "cruiser":
                    cCruiserCount--;
                    break;
                case "submarine":
                    cSubmarineCount--;
                    break;
                case "battleship":
                    cBattleshipCount--;
                    break;
                case "carrier":
                    cCarrierCount--;
                    break;
            }
            square.Tag = "hit";
            square.Background = new SolidColorBrush(Colors.Red);
            turnCount++;
            compTurn();
            checkWinner();

        }

        private void compTurn()
        {
            Random random = new Random();
            if (difficulty == Difficulty.Simple)
            {
                
            }
            else
            {

            }
            turnCount++;
        }
        private void checkWinner()
        {
            if (cCarrierCount == 0)
            {
                cCarrierCount = -1;
                MessageBox.Show("You sunk my Aircraft Carrier!");
            }
            if (cCruiserCount == 0)
            {
                cCruiserCount = -1;
                MessageBox.Show("You sunk my Cruiser!");
            }
            if (cDestroyerCount == 0)
            {
                cDestroyerCount = -1;
                MessageBox.Show("You sunk my Destroyer!");
            }
            if (cBattleshipCount == 0)
            {
                cBattleshipCount = -1;
                MessageBox.Show("You sunk my Battleship!");
            }
            if (cSubmarineCount == 0)
            {
                cSubmarineCount = -1;
                MessageBox.Show("You sunk my Submarine!");
            }
            if (cCarrierCount == -1 && cBattleshipCount == -1 && cSubmarineCount == -1 && 
                cCruiserCount == -1 && cDestroyerCount == -1)
            {
                MessageBox.Show("You winnnnnnnn");
            }
        }

        /// <summary>
        /// Validates X coordinate.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <returns>char X coordinate if good. Otherwise char '-'</returns>
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

        /// <summary>
        /// Validate Y coordinate
        /// </summary>
        /// <param name="Y">Y coordinate</param>
        /// <returns>char Y coordinate if good. Otherwise char '-'</returns>
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

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            char X = validateXCoordinate(txtBoxX.Text);
            char Y = validateYCoordinate(txtBoxY.Text);

            if (X == '-' || Y == '-')
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //TODO 
        }

    }
}
