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

        public Grid[] grid;

        private bool _isShipDragInProg;
 
        public ShipPlacement()
        {
            InitializeComponent();
        }

        private void ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isShipDragInProg = true;
            ship.CaptureMouse();
        }

        private void ship_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isShipDragInProg = false;
            ship.ReleaseMouseCapture();
        }

        private void ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isShipDragInProg) return;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(canvas);

            // center the rect on the mouse
            double left = mousePos.X - (ship.ActualWidth / 2);
            double top = mousePos.Y - (ship.ActualHeight / 2);
            Canvas.SetLeft(ship, left);
            Canvas.SetTop(ship, top);
        }
    }
}
