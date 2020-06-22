using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.ViewModels;
using MattEland.FSharpStarship.Logic;

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

            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(250), 
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

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _vm.HandlePlayerCommand(PlayerControl.PlayerCommand.NewMove(Positions.Direction.Up));
                    e.Handled = true;
                    break;
                case Key.Right:
                    _vm.HandlePlayerCommand(PlayerControl.PlayerCommand.NewMove(Positions.Direction.Right));
                    e.Handled = true;
                    break;
                case Key.Down:
                    _vm.HandlePlayerCommand(PlayerControl.PlayerCommand.NewMove(Positions.Direction.Down));
                    e.Handled = true;
                    break;
                case Key.Left:
                    _vm.HandlePlayerCommand(PlayerControl.PlayerCommand.NewMove(Positions.Direction.Left));
                    e.Handled = true;
                    break;
                case Key.Space:
                    _vm.HandlePlayerCommand(PlayerControl.PlayerCommand.Wait);
                    e.Handled = true;
                    break;
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            gridMain.Focus();
        }

        private void GridMain_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridMain.Focus();
        }
    }
}
