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
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : UserControl
    {
        public event EventHandler play;
        
        enum Orientation { VERTICAL, HORIZONTAL};
        Orientation orientation = Orientation.HORIZONTAL;
        SolidColorBrush unselected = new SolidColorBrush(Colors.Black);
        SolidColorBrush selected = new SolidColorBrush(Colors.Green);
        String ship = "";
        int size;
        int numShipsPlaced;
        Path lastShip;
        Path[] ships;
        Polygon lastArrow;
        public Grid[] playerGrid;

        SolidColorBrush[] shipColors = new SolidColorBrush[] {(SolidColorBrush)(new BrushConverter().ConvertFrom("#88cc00")), (SolidColorBrush)(new BrushConverter().ConvertFrom("#33cc33")),
                                                                  (SolidColorBrush)(new BrushConverter().ConvertFrom("#00e64d")),(SolidColorBrush)(new BrushConverter().ConvertFrom("#00cc00")),
                                                                  (SolidColorBrush)(new BrushConverter().ConvertFrom("#00e600"))};

        public ShipPlacement()
        {
            InitializeComponent();
            playerGrid = new Grid[] { gridA1, gridA2, gridA3, gridA4, gridA5, gridA6, gridA7,gridA8,gridA9,gridA10,
                                gridB1, gridB2, gridB3, gridB4, gridB5, gridB6, gridB7,gridB8,gridB9,gridB10,
                                gridC1, gridC2, gridC3, gridC4, gridC5, gridC6, gridC7,gridC8,gridC9,gridC10,
                                gridD1, gridD2, gridD3, gridD4, gridD5, gridD6, gridD7,gridD8,gridD9,gridD10,
                                gridE1, gridE2, gridE3, gridE4, gridE5, gridE6, gridE7,gridE8,gridE9,gridE10,
                                gridF1, gridF2, gridF3, gridF4, gridF5, gridF6, gridF7,gridF8,gridF9,gridF10,
                                gridG1, gridG2, gridG3, gridG4, gridG5, gridG6, gridG7,gridG8,gridG9,gridG10,
                                gridH1, gridH2, gridH3, gridH4, gridH5, gridH6, gridH7,gridH8,gridH9,gridH10,
                                gridI1, gridI2, gridI3, gridI4, gridI5, gridI6, gridI7,gridI8,gridI9,gridI10,
                                gridJ1, gridJ2, gridJ3, gridJ4, gridJ5, gridJ6, gridJ7,gridJ8,gridJ9,gridJ10 };
            ships = new Path[] { destroyer, cruiser,submarine,battleship,carrier };
            reset();
            
        }

        /// <summary>
        /// Reset the setDown grid.
        /// Tags: 
        ///     0. water
        ///     1. destroyer
        ///     2. cruiser
        ///     3. submarine
        ///     4. battleship
        ///     5. carrier
        /// </summary>
        private void reset()
        {
            if (lastArrow != null)
            {
                lastArrow.Stroke = unselected;
            }
            lastArrow = rightPoly;
            rightPoly.Stroke = selected;

            foreach (var element in playerGrid)
            {
                element.Tag = "water";
                element.Background = new SolidColorBrush(Colors.White);
            }

            foreach (var element in ships)
            {
                element.IsEnabled = true;
                element.Opacity = 100;
                if (element.Stroke != unselected)
                {
                    element.Stroke = unselected;
                }
            }
            numShipsPlaced = 0;
            lastShip = null;
        }

        /// <summary>
        /// When the ship format is clicked, make it show and
        /// put it to global variable ship.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Path shipPath = (Path)sender;
            if (!shipPath.IsEnabled)
            {
                return;
            }
            if (lastShip != null)
            {
                lastShip.Stroke = unselected;
            }

            lastShip = shipPath;
            ship = shipPath.Name;
            shipPath.Stroke = selected;

            switch(ship)
            {
                case "carrier":
                    size = 5;
                    break;
                case "battleship":
                    size = 4;
                    break;
                case "submarine":
                case "cruiser":
                    size = 3;
                    break;
                case "destroyer":
                    size = 2;
                    break;
            }
        }
        /// <summary>
        /// When the orientation arrow (left,right,Down,down) is selected
        /// make it show and change the orientation enum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orientationMouseDown(object sender, MouseButtonEventArgs e)
        {
            Polygon arrow = (Polygon)sender;

            lastArrow.Stroke = unselected;
            lastArrow = arrow;
            arrow.Stroke = selected;

            if (arrow.Name.Equals("rightPoly") || arrow.Name.Equals("leftPoly"))
            {
                orientation = Orientation.HORIZONTAL;
            }
            else
            {
                orientation = Orientation.VERTICAL;
            }
        }

        /// <summary>
        /// When grid square is clicked, determine if a ship should
        /// be placed there and if yes, place it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid square = (Grid)sender;
            int index = -1;
            int temp;
            int counter = 1;

            //Check if ship has been selected
            if (lastShip == null)
            {
                MessageBox.Show("You must choose a ship", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check if square has a ship already in place
            if (!square.Tag.Equals("water"))
            {
                return;
            }

            //Find chosen square. Index should never be -1.
            index = Array.IndexOf(playerGrid, square);

            //Check if there is enough space for the ship

            if (orientation.Equals(Orientation.HORIZONTAL))
            {
                try {
                    counter = 1;
                    for (int i = 0; i < size; i++)
                    {
                        //This sees if the index is within the grid going ---->
                        if (index + i <= 99)
                        {
                            if (!playerGrid[index + i].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid ship placement, not enough space!");
                            }
                        }
                        //Goes <---- to see if there is space
                        else
                        {
                            if (!playerGrid[index-counter].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid ship placement"); 
                            }
                            counter++;
                        }

                    }
                } catch (IndexOutOfRangeException iore)
                {
                    MessageBox.Show(iore.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            else //for orientation down
            {
                try {
                    counter = 10;
                    for (int i = 0; i < size * 10; i += 10)
                    {
                        if (index + i <= 99)
                        {
                            if (!playerGrid[index + i].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid ship placement!");
                            }
                        }
                        else
                        {
                            if (!playerGrid[index - counter].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid ship placement! Wrong counter.");
                            }
                            counter += 10;
                        }
                    }
                    if ((index / 10) + (size * 10) > 100)
                    {
                        throw new IndexOutOfRangeException("Invalid ship placement, not enough space!");
                    }
                } catch (IndexOutOfRangeException iore)
                {
                    MessageBox.Show(iore.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            
            //Set the ship to grid
            if (orientation.Equals(Orientation.HORIZONTAL))
            {
                //If two rows
                if ((index + size - 1) % 10 < size - 1)
                {
                    counter = 0;
                    temp = 1;

                    while ((index + counter) % 10 > 1)
                    {
                        playerGrid[index + counter].Background = selectColor();
                        playerGrid[index + counter].Tag = ship;
                        counter++;
                    }
                    for (int i = counter; i < size; i++)
                    {
                        playerGrid[index - temp].Background = selectColor();
                        playerGrid[index - temp].Tag = ship;
                        temp++;
                    }
                }
                //If one row
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        playerGrid[index + i].Background = selectColor();
                        playerGrid[index + i].Tag = ship;
                    }
                }
            }
            else
            {
                //If two columns
                if (index + (size * 10) > 100)
                {
                    counter = 0;
                    temp = 10;
                    while ((index / 10 + counter ) % 100 < 10)
                    {
                        playerGrid[index + counter * 10].Background = selectColor();
                        playerGrid[index + counter * 10].Tag = ship;
                        counter++;
                    }
                    for (int i = counter; i < size; i++)
                    {
                        playerGrid[index - temp].Background = selectColor();
                        playerGrid[index - temp].Tag = ship;
                        temp += 10;
                    }
                }
                //If one column
                else
                {
                    counter = 0;
                    for (int i = 0; i  < size * 10; i += 10)
                    {
                        playerGrid[index + i].Background = selectColor();
                        playerGrid[index + i].Tag = ship;
                    }
                }
            }
            lastShip.IsEnabled = false;
            lastShip.Opacity = 0.5;
            lastShip.Stroke = unselected;
            lastShip = null;
            numShipsPlaced++;
        }

        /// <summary>
        /// Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (numShipsPlaced != 5)
            {
                return;
            }
            play(this,e);
        }

        /// <summary>
        /// Reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        /// <summary>
        /// Sets grid randomnly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRandomize_Click(object sender, RoutedEventArgs e)
        {
            reset();
            Random random = new Random();
            int[] shipSizes = new int[] { 2, 3, 3, 4, 5 };
            string[] shipNames = new string[] { "destroyer", "cruiser", "submarine", "battleship", "carrier" };
            int size, index;
            string ship;
            Orientation orientation;
            bool unavailableIndex = true;
            

            for (int i = 0; i < shipSizes.Length; i++)
            {
                //Set size and ship type
                size = shipSizes[i];
                ship = shipNames[i];
                unavailableIndex = true;

                if (random.Next(0, 2) == 0)
                    orientation = Orientation.HORIZONTAL;
                else
                    orientation = Orientation.VERTICAL;

                //Set ships
                if (orientation.Equals(Orientation.HORIZONTAL))
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
                            if (index + j > 99 || !playerGrid[index + j].Tag.Equals("water"))
                            {
                                index = random.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < size; j++)
                    {
                        playerGrid[index + j].Tag = ship;
                        playerGrid[index + j].Background = shipColors[i];
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
                            if (index + j > 99 || !playerGrid[index + j].Tag.Equals("water"))
                            {
                                index = random.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < size * 10; j += 10)
                    {
                        playerGrid[index + j].Tag = ship;
                        playerGrid[index + j].Background = shipColors[i];
                    }
                }

            }
            numShipsPlaced = 5;
            foreach (var element in ships)
            {
                element.IsEnabled = false;
                element.Opacity = .5;
                if (element.Stroke != unselected)
                {
                    element.Stroke = unselected;
                }

            }
        }

        /// <summary>
        /// Choose the background
        /// </summary>
        /// <returns></returns>
        private SolidColorBrush selectColor()
        {
            switch (ship)
            {
                case "destroyer":
                    return shipColors[0];
                case "cruiser":
                    return shipColors[1];
                case "submarine":
                    return shipColors[2];
                case "carrier":
                    return shipColors[3];
                case "battleship":
                    return shipColors[4];
            }
            return shipColors[0];
        }
       
    }
}
