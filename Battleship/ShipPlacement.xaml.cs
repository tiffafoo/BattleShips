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
        
        enum Orientation { Down, Right};
        Orientation orientation = Orientation.Right;
        SolidColorBrush unselected = new SolidColorBrush(Colors.Black);
        SolidColorBrush selected = new SolidColorBrush(Colors.Green);
        String ship = "";
        int size;
        Path lastShip;
        Polygon lastArrow;
        public Grid[] grid;
 
        public ShipPlacement()
        {
            InitializeComponent();
            grid = new Grid[] { gridA1, gridA2, gridA3, gridA4, gridA5, gridA6, gridA7,gridA8,gridA9,gridA10,
                                gridB1, gridB2, gridB3, gridB4, gridB5, gridB6, gridB7,gridB8,gridB9,gridB10,
                                gridC1, gridC2, gridC3, gridC4, gridC5, gridC6, gridC7,gridC8,gridC9,gridC10,
                                gridD1, gridD2, gridD3, gridD4, gridD5, gridD6, gridD7,gridD8,gridD9,gridD10,
                                gridE1, gridE2, gridE3, gridE4, gridE5, gridE6, gridE7,gridE8,gridE9,gridE10,
                                gridF1, gridF2, gridF3, gridF4, gridF5, gridF6, gridF7,gridF8,gridF9,gridF10,
                                gridG1, gridG2, gridG3, gridG4, gridG5, gridG6, gridG7,gridG8,gridG9,gridG10,
                                gridH1, gridH2, gridH3, gridH4, gridH5, gridH6, gridH7,gridH8,gridH9,gridH10,
                                gridI1, gridI2, gridI3, gridI4, gridI5, gridI6, gridI7,gridI8,gridI9,gridI10,
                                gridJ1, gridJ2, gridJ3, gridJ4, gridJ5, gridJ6, gridJ7,gridJ8,gridJ9,gridJ10 };
            reset();
            foreach (var element in grid)
            {
                Console.WriteLine(element.Name);
            }
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
            lastArrow = rightPoly;
            rightPoly.Stroke = selected;
            foreach (var element in grid)
            {
                element.Tag = "water";
            }
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
                orientation = Orientation.Right;
            }
            else
            {
                orientation = Orientation.Down;
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
            index = Array.IndexOf(grid,square);
            
            //Check if there is enough space for the ship
            if (orientation.Equals(Orientation.Right))
            {
                try {
                    for (int i = 0; i < size; i++)
                    {
                        if (!grid[index + i].Tag.Equals("water"))
                        {
                            throw new IndexOutOfRangeException("Invalid ship placement, not enough space!");
                        }
                    }
                    if ((index + size - 1) % 10 < size - 1)
                    {
                        throw new IndexOutOfRangeException("Invalid ship placement, not enough space!");
                    }
                } catch (IndexOutOfRangeException iore)
                {
                    MessageBox.Show(iore.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            else //for orientation down
            {
                for (int i = 0; i < size * 10; i +=10)
                {
                    if (index + i > 99 || !grid[index+i].Tag.Equals("water"))
                    {
                        MessageBox.Show("Invalid ship placement","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if ((index/10) + (size * 10) > 100)
                {
                    MessageBox.Show("Invalid ship placement, not enough space!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            MessageBox.Show(size.ToString());
            if (orientation.Equals(Orientation.Right))
            {
                for (int i = 0; i< size; i++)
                {
                    grid[index + i].Background = selected;
                    grid[index + i].Tag = ship;
                }
            }
            else
            {
                for(int i = 0; i < size * 10; i += 10)
                {
                    grid[index + i].Background = selected;
                    grid[index + i].Tag = ship;
                }
            }
            lastShip.IsEnabled = false;
            lastShip.Opacity = 0.5;
            lastShip.Stroke = unselected;
            lastShip = null;
        }
    }
}
