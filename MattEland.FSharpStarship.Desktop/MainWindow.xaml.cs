using System.Windows;
using MattEland.FSharpStarship.Desktop.ViewModels;

namespace MattEland.FSharpStarship.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private void AdvanceTime_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.AdvanceTime();
        }
    }
}
