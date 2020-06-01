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
using MattEland.FSharpStarship.Desktop.ViewModels;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IEnumerable<TileViewModel> Tiles { get; }

        public MainWindow()
        {
            InitializeComponent();

            Tiles = Common.getTiles().Select(t => new TileViewModel(t));
            DataContext = this;
        }
    }
}
