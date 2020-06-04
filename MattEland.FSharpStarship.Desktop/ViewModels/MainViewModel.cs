using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private View.AppView _view;

        public MainViewModel()
        {
            GameWorld = WorldBuilding.generateWorld();
            Tiles = GameWorld.tiles.Select(t => new TileViewModel(t, this)).ToList();
            Objects = GameWorld.objects.Select(t => new GameObjectViewModel(t, this)).ToList();
            _view = View.getDefaultAppView();
        }

        public World.GameWorld GameWorld { get; }

        public List<TileViewModel> Tiles { get; }
        public List<GameObjectViewModel> Objects { get; }

        public IEnumerable<string> ViewModes => Enum.GetNames(typeof(View.CurrentOverlay));

        public string SelectedViewMode
        {
            get => _view.overlay.ToString();
            set
            {
                if (value == _view.overlay.ToString()) return;

                var newEnum = Enum.Parse<View.CurrentOverlay>(value);
                _view = View.changeOverlay(_view, newEnum);
                OnPropertyChanged();

                Tiles.ForEach(t => t.HandleOverlayChanged());
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

                Tiles.ForEach(t => t.HandleOverlayChanged());
            }
        }
    }
}
