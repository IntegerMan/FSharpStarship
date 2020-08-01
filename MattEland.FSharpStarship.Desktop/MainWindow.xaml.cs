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

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel();
            DataContext = _vm;
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

        private void OnResetClicked(object sender, RoutedEventArgs e)
        {
            _vm.ResetGame();
        }
    }
}
