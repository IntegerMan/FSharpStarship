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
        public MainViewModel()
        {
            _view = View.getDefaultAppView();
            string dir = Environment.CurrentDirectory;
            int index = dir.IndexOf(@"\bin\", StringComparison.OrdinalIgnoreCase);
            dir = dir.Substring(0, index);

            UpdateTiles(TiledInterop.loadWorld($@"{dir}\FSharpStarship.tmx"));
        }

        private void UpdateTiles(IEnumerable<Tiles.Tile> tiles)
        {
            // Ensure we lose state from prior run
            Tiles.Clear();

            // Add Tiles and Gas Particles
            tiles.Select(t => new TileViewModel(t, this)).ToList().ForEach(t => Tiles.Add(t));

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

        public void AdvanceTime()
        {
            var tiles = ListModule.OfSeq(this.Tiles.Select(t => t.Tile));
            UpdateTiles(Simulations.simulate(tiles));
        }
    }
}
