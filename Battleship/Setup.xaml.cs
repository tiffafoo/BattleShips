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

    public enum Difficulty { Simple, Intelligent }

    public partial class Setup : UserControl
    {
        public event EventHandler play;
        public string name;
        public Difficulty difficulty = Difficulty.Simple;

        public Setup()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            name = txtboxName.Text;
            if (name == "")
            {
                MessageBox.Show("You must enter a name", "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                play(this,e);
            }
        }

        private void rbtnSimple_Click(object sender, RoutedEventArgs e)
        {
            difficulty = Difficulty.Simple;
        }

        private void rbtnIntelligent_Click(object sender, RoutedEventArgs e)
        {
            difficulty = Difficulty.Intelligent;
        }
    }
}
