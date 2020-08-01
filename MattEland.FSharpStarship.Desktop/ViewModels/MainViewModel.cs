using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MattEland.FSharpStarship.Logic;
using Microsoft.FSharp.Collections;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private View.AppView _view;
        private FSharpList<Tiles.Tile> _tiles;

        public MainViewModel()
        {
            _view = View.getDefaultAppView();
            ResetGame();
        }

        private void UpdateTiles(FSharpList<Tiles.Tile> tiles)
        {
            if (_tiles == null)
            {
                _tiles = tiles;
                // Ensure we lose state from prior run
                Tiles.Clear();

                // Add Tiles and Gas Particles
                tiles.Select(t => new TileViewModel(t, this)).ToList().ForEach(t => Tiles.Add(t));
            }
            else
            {
                _tiles = tiles;

                foreach (var tile in tiles)
                {
                    Tiles.First(t => t.Tile.Pos.Equals(tile.Pos)).Tile = tile;
                }
            }

            OnPropertyChanged();
        }

        public ObservableCollection<TileViewModel> Tiles { get; } = new ObservableCollection<TileViewModel>();

        public IEnumerable<string> ViewModes => Enum.GetNames(typeof(View.CurrentOverlay));

        public string SelectedViewMode
        {
            get => _view.Overlay.ToString();
            set
            {
                if (value == _view.Overlay.ToString()) return;

                var newEnum = Enum.Parse<View.CurrentOverlay>(value);
                _view = View.changeOverlay(_view, newEnum);
                OnPropertyChanged();

                Tiles.ToList().ForEach(t => t.HandleOverlayChanged());
            }
        }

        public View.AppView AppView
        {
            get => _view;
            set
            {
                if (Equals(value, _view)) return;
                _view = value;
                OnPropertyChanged();

                Tiles.ToList().ForEach(t => t.HandleOverlayChanged());
            }
        }

        public void AdvanceTime() => UpdateTiles(Simulations.simulate(_tiles));

        public void HandlePlayerCommand(PlayerControl.PlayerCommand command) 
            => UpdateTiles(PlayerControl.handlePlayerCommand(command, _tiles));

        public void ResetGame()
        {
            string dir = Environment.CurrentDirectory;
            int index = dir.IndexOf(@"\bin\", StringComparison.OrdinalIgnoreCase);
            dir = dir.Substring(0, index);
            UpdateTiles(TiledInterop.loadWorld($@"{dir}\FSharpStarship.tmx"));
        }
    }
}
