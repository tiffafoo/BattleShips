using System;
using System.Collections.Generic;
using System.IO;
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
        public event EventHandler replay;

        public Difficulty difficulty;
        public string playerName;
        public int highScore;
        public Grid[] playerGrid;
        public Grid[] compGrid;
        public List<int> hitList;
        int turnCount = 0;
        public Random random = new Random();

        int pCarrierCount = 5, cCarrierCount = 5;
        int pBattleshipCount = 4, cBattleshipCount = 4;
        int pSubmarineCount = 3, cSubmarineCount = 3;
        int pCruiserCount = 3, cCruiserCount = 3;
        int pDestroyerCount = 2, cDestroyerCount = 2;

        public PlayVSComp(Difficulty difficulty, Grid[] playerGrid, string playerName)
        {
            InitializeComponent();

            this.playerName = playerName;
            this.difficulty = difficulty;
            initiateSetup(playerGrid);
            hitList = new List<int>();
            displayHighScores(loadHighScores());

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
            btnAttack.IsEnabled = true;
        }

        /// <summary>
        /// Initiate the computer's grid
        /// </summary>
        private void setupCompGrid()
        {
            Random random = new Random();
            int[] shipSizes = new int[] { 2, 3, 3, 4, 5 };
            string[] ships = new string[] { "destroyer", "cruiser", "submarine", "battleship", "carrier" };
            int size, index;
            string ship;
            Orientation orientation;
            bool unavailableIndex = true;

            for (int i = 0; i < shipSizes.Length; i++)
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
                                index = random.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < size; j++)
                    {
                        compGrid[index + j].Tag = ship;
                        //compGrid[index + j].Background = new SolidColorBrush(Colors.LightGreen); //remove after testing
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
                        //compGrid[index + j].Background = new SolidColorBrush(Colors.LightGreen); //remove after testing
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
                    square.Background = new SolidColorBrush(Colors.LightGray);
                    turnCount++;
                    compTurn();
                    return;
                case "miss":
                case "hit":
                    Console.WriteLine("User hit a miss/hit");
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
            checkPlayerWin();
            compTurn();

        }

        private void compTurn()
        {
            if (difficulty == Difficulty.Simple)
            {
                hunterMode();
            }
            else
            {
                intelligentMoves();
            }
            turnCount++;
            checkComputerWin();
        }
        private void checkPlayerWin()
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
                MessageBox.Show("You win!");
                disableGrids();
                displayHighScores(saveHighScores(true));
            }
        }

        

        private void checkComputerWin()
        {
            if (pCarrierCount == 0)
            {
                pCarrierCount = -1;
                MessageBox.Show("Your Aircraft Carrier got destroyed!");
            }
            if (pCruiserCount == 0)
            {
                pCruiserCount = -1;
                MessageBox.Show("Your Cruiser got destroyed!");
            }
            if (pDestroyerCount == 0)
            {
                pDestroyerCount = -1;
                MessageBox.Show("Your Destroyer got destroyed!");
            }
            if (pBattleshipCount == 0)
            {
                pBattleshipCount = -1;
                MessageBox.Show("Your Battleship got destroyed!");
            }
            if (pSubmarineCount == 0)
            {
                pSubmarineCount = -1;
                MessageBox.Show("Your Submarine got destroyed!");
            }

            if (pCarrierCount == -1 && pBattleshipCount == -1 && pSubmarineCount == -1 &&
                pCruiserCount == -1 && pDestroyerCount == -1)
            {
                MessageBox.Show("You lose!");
                disableGrids();
                displayHighScores(saveHighScores(false));
            }
        }
        private void disableGrids()
        {
            foreach (var element in compGrid)
            {
                if (element.Tag.Equals("water"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                else if (element.Tag.Equals("carrier") || element.Tag.Equals("cruiser") ||
                  element.Tag.Equals("destroyer") || element.Tag.Equals("battleship") || element.Tag.Equals("submarine"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGreen);
                }
                element.IsEnabled = false;
            }
            foreach (var element in playerGrid)
            {
                if (element.Tag.Equals("water"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                element.IsEnabled = false;
            }
            clearTextBoxes();
            btnAttack.IsEnabled = false;

        }
        private string validateXCoordinate(string X)
        {
            if (X.Length != 1)
            {
                return "";
            }

            if (Char.IsLetter(X[0]))
            {
                return X;
            }
            return "";
        }

        /// <summary>
        /// Validate Y coordinate
        /// </summary>
        /// <param name="Y">Y coordinate</param>
        /// <returns>char Y coordinate if good. Otherwise char '-'</returns>
        private string validateYCoordinate(string Y)
        {
            if (Y.Length > 2 || Y == "")
            {
                return "";
            }

            if (int.Parse(Y) > 0 || int.Parse(Y) <= 10)
            {
                return Y;
            }
            return "";
        }

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            string X = validateXCoordinate(txtBoxX.Text);
            string Y = validateYCoordinate(txtBoxY.Text);
            int index = 0;

            if (X == "" || Y == "")
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (X)
            {
                case "A":
                    index = 0;
                    break;
                case "B":
                    index = 10;
                    break;
                case "C":
                    index = 20;
                    break;
                case "D":
                    index = 30;
                    break;
                case "E":
                    index = 40;
                    break;
                case "F":
                    index = 50;
                    break;
                case "G":
                    index = 60;
                    break;
                case "H":
                    index = 70;
                    break;
                case "I":
                    index = 80;
                    break;
                case "J":
                    index = 90;
                    break;
            }
            index += int.Parse(Y) - 1;
            clearTextBoxes();
            gridMouseDown(compGrid[index], null);

        }

        private void clearTextBoxes()
        {
            txtBoxX.Text = "";
            txtBoxY.Text = "";
        }
        private void btnStartOver_Click(object sender, RoutedEventArgs e)
        {
            replay(this, e);
        }

        private void btnLetter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            txtBoxX.Text = button.Content.ToString();
        }

        private void btnNumber_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            txtBoxY.Text = button.Content.ToString();
        }

        /// <summary>
        /// Computer AI moves based on if it is in "Hunter" mode
        /// (has not found a ship) or "Killer" mode (is attempting
        /// to destroy a ship).
        /// </summary>
        private void intelligentMoves()
        {
            // If there are no squares to hit
            if (hitList.Count == 0)
            {
                Console.WriteLine("hitlist is empty");
                hunterMode();
            }
            // assumes there is a ship found
            else
                killerMode();
        }

        /// <summary>
        /// Hunter Mode fires randoming, attempting to find a ship
        /// </summary>
        private void hunterMode()
        {
            int position;
            do
            {
                position = random.Next(100);
                Console.WriteLine(playerGrid[position].Tag);
                Console.WriteLine("Randomizing position");
            } while ((playerGrid[position].Tag.Equals("miss")) || (playerGrid[position].Tag.Equals("hit")));


            if (difficulty == Difficulty.Simple)
            {
                Console.WriteLine("Going simple");
                simpleMode(position);
            }
            else
            {
                fireAtLocation(position);
            }

        }

        /// <summary>
        /// Simple Difficulty fires randomly with no other algorithm
        /// </summary>
        /// <param name="position"></param>
        private void simpleMode(int position)
        {
            if (!(playerGrid[position].Tag.Equals("water")))
            {
                // If ship is hit mark it down
                switch (playerGrid[position].Tag.ToString())
                {
                    case "destroyer":
                        pDestroyerCount--;
                        break;
                    case "cruiser":
                        pCruiserCount--;
                        break;
                    case "submarine":
                        pSubmarineCount--;
                        break;
                    case "battleship":
                        pBattleshipCount--;
                        break;
                    case "carrier":
                        pCarrierCount--;
                        break;
                }
                // Mark the grid as hit
                playerGrid[position].Tag = "hit";
                playerGrid[position].Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                playerGrid[position].Tag = "miss";
                playerGrid[position].Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        /// <summary>
        /// Determines if the shot is a hit or miss. In the event
        /// of a hit the ship is checked if it's destroyed or not
        /// and if so checks for a winner before going back to hunter
        /// mode. In the event of a miss the grid is changed to
        /// reflect that
        /// </summary>
        /// <param name="position"></param>
        private void fireAtLocation(int position)
        {
            //If the position contains one of the ships (therefore, not water, missed shot, or already hit ship)
            if (!(playerGrid[position].Tag.Equals("water")))
            {
                // If this grid is in the hitList, remove it
                if (hitList != null && hitList.Contains(position))
                    hitList.Remove(position);

                // If ship is hit mark it down
                switch (playerGrid[position].Tag.ToString())
                {
                    case "destroyer":
                        pDestroyerCount--;
                        break;
                    case "cruiser":
                        pCruiserCount--;
                        break;
                    case "submarine":
                        pSubmarineCount--;
                        break;
                    case "battleship":
                        pBattleshipCount--;
                        break;
                    case "carrier":
                        pCarrierCount--;
                        break;
                }
                // Mark the grid as hit
                playerGrid[position].Tag = "hit";
                playerGrid[position].Background = new SolidColorBrush(Colors.Red);

                // If a ship is destroyed clear the hitList to return to Hunter Mode
                if (pDestroyerCount == 0 || pCruiserCount == 0 || pSubmarineCount == 0 || pBattleshipCount == 0 || pCarrierCount == 0)
                {
                    hitList.Clear();
                }
                // If a ship is not destroyed add adjacent grids to hitList
                else
                {
                    // Computer hit a ship, add the adjacent grids to hitList
                    // If the position is on the left side
                    if (position % 10 == 0)
                        hitList.Add(position + 1);
                    // If the position is on the  right side
                    else if (position % 10 == 9)
                        hitList.Add(position - 1);
                    // Is the position is not on the left or right
                    else
                    {
                        hitList.Add(position + 1);
                        hitList.Add(position - 1);
                    }
                    // If the position is on the top
                    if (position < 10)
                        hitList.Add(position + 10);
                    // If the position is on the bottom
                    else if (position > 89)
                        hitList.Add(position - 10);
                    // If the position is not on the top or bottom
                    else
                    {
                        hitList.Add(position + 10);
                        hitList.Add(position - 10);
                    }

                    // The following code should improve the AI's options by removing squares that are likely to be misses
                    try
                    {
                        hitList.Remove(position - 11);
                    }
                    catch (Exception e) { }
                    try
                    {
                        hitList.Remove(position - 9);
                    }
                    catch (Exception e) { }
                    try
                    {
                        hitList.Remove(position + 9);
                    }
                    catch (Exception e) { }
                    try
                    {
                        hitList.Remove(position + 11);
                    }
                    catch (Exception e) { }
                }
            }
            else
            {
                playerGrid[position].Tag = "miss";
                playerGrid[position].Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        /// <summary>
        /// Clears the score list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            string path = @"../../scores.txt";
            File.Delete(path);
            FileStream stream = File.Create(path);
            stream.Close();

            txtBlockNames.Text = "NAME";
            txtBlockWins.Text = "WINS";
            txtBlockLosses.Text = "LOSSES";
        }

        /// <summary>
        /// Fire on one of the grid squares from the hitList
        /// </summary>
        private void killerMode()
        {
            int position;
            // Prepare to fight at a random grid of the hitList
            do
            {
                Console.WriteLine("killerMode loop");
                position = random.Next(hitList.Count);
            } while (playerGrid[hitList[position]].Tag.Equals("miss") || playerGrid[hitList[position]].Tag.Equals("hit"));

            //Find the index for the grid in the Grid Array and fire
            Console.WriteLine("HitList count: " + hitList.Count);
            Console.WriteLine(hitList);
            fireAtLocation(hitList[position]);

        }

        /// <summary>
        /// Save high scores
        /// </summary>
        /// <param name="playerWins"></param>
        /// <returns></returns>
        private List<string> saveHighScores(bool playerWins)
        {
            String filename = @"../../scores.txt";
            string[] user = { playerName, "0", "0" };
            string[] playerNames;
            int index;
            int wins = 0;
            int losses = 0;

            //Create file if it doesn't exists
            if (!File.Exists(filename))
            {
                FileStream stream = File.Create(filename);
                stream.Close();
            }

            //Get list of players
            List<string> players = new List<string>(File.ReadAllLines(filename));

            playerNames = new string[players.Count];

            for (index = 0; index < players.Count; index++)
            {
                playerNames[index] = players[index].Split(' ')[0];
            }
            //See if player exists already
            index = binarySearch(playerNames, playerName);

            if (index > -1)
            {
                user = players[index].Split();
                players.RemoveAt(index);
            }
            else
            {
                index = -(index + 1);
            }
            if (playerWins == true)
            {
                wins = int.Parse(user[1]) + 1;
            }
            else
            {
                losses = int.Parse(user[2]) + 1;
            }
            players.Insert(index, playerName + " " + wins + " " + losses);

            File.WriteAllLines(filename, players);
            return players;
        }
        /// <summary>
        /// Binary search that returns index of where something should be
        /// </summary>
        /// <param name="players"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int binarySearch(string[] players, string value)
        {

            int low = 0;
            int high = players.Length - 1;

            while (high >= low)
            {
                int middle = (low + high) / 2;

                if (players[middle].CompareTo(value) == 0)
                {
                    return middle;
                }
                if (players[middle].CompareTo(value) < 0)
                {
                    low = middle + 1;
                }
                if (players[middle].CompareTo(value) > 0)
                {
                    high = middle - 1;
                }
            }
            return -(low + 1);
        }

        /// <summary>
        /// Load the high scores (initiation)
        /// </summary>
        /// <returns></returns>
        private List<string> loadHighScores()
        {
            String filename = @"../../scores.txt";
            string[] playerNames;
            int index;

            //Create file if it doesn't exists
            if (!File.Exists(filename))
            {
                FileStream stream = File.Create(filename);
                stream.Close();
            }

            //Get list of players
            List<string> players = new List<string>(File.ReadAllLines(filename));

            playerNames = new string[players.Count];

            for (index = 0; index < players.Count; index++)
            {
                playerNames[index] = players[index].Split(' ')[0];
            }

            File.WriteAllLines(filename, players);
            return players;
        }

        /// <summary>
        /// Display the High Scores
        /// </summary>
        /// <param name="players"></param>
        private void displayHighScores(List<string> players)
        {
            string[] player;
            string names = "Name" + Environment.NewLine;
            string wins = "Wins" + Environment.NewLine;
            string losses = "Losses" + Environment.NewLine;

            for (int i = 0; i < players.Count; i++)
            {
                player = players[i].Split(' ');
                names += player[0] + Environment.NewLine;
                wins += player[1] + Environment.NewLine;
                losses += player[2] + Environment.NewLine;
            }
            txtBlockNames.Text = names;
            txtBlockWins.Text = wins;
            txtBlockLosses.Text = losses;

        }
    }
}
