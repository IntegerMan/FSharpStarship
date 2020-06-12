using System;
using System.Windows;
using System.Windows.Threading;
using MattEland.FSharpStarship.Desktop.Annotations;
using MattEland.FSharpStarship.Desktop.ViewModels;

namespace MattEland.FSharpStarship.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [UsedImplicitly]
    public partial class MainWindow
    {
        private readonly MainViewModel _vm;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel();
            DataContext = _vm;

            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), 
                                         DispatcherPriority.Background, 
                                         (sender, e) => _vm.AdvanceTime(), 
                                         Dispatcher.CurrentDispatcher);
            _timer.Stop();
        }

        private void OnTogglePauseClick(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                togglePause.Content = "Play";
                togglePause.IsChecked = false;
            }
            else
            {
                _timer.Start();
                togglePause.Content = "Pause";
                togglePause.IsChecked = true;
            }
        }
    }
}
