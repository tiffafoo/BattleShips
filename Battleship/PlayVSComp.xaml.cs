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
        private Difficulty difficulty;
        private Grid[] grid;

        public PlayVSComp()
        {
            InitializeComponent();
        }

        public PlayVSComp(Difficulty difficulty, Grid[] grid)
        {
            this.difficulty = difficulty;
            this.grid = grid;
        }
    }
}
