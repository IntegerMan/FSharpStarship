using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private View.AppView _view;
        private World.GameWorld _gameWorld;

        public MainViewModel()
        {
            GameWorld = WorldBuilding.generateWorld();
            _view = View.getDefaultAppView();
        }

        public World.GameWorld GameWorld
        {
            get => _gameWorld;
            set
            {
                if (Equals(value, _gameWorld)) return;
                _gameWorld = value;

                Tiles.Clear();
                GameWorld.tiles.Select(t => new TileViewModel(t, this)).ToList().ForEach(t => Tiles.Add(t));

                Objects.Clear();
                GameWorld.objects.Select(t => new GameObjectViewModel(t, this)).ToList().ForEach(o => Objects.Add(o));

                OnPropertyChanged();
            }
        }

        public ObservableCollection<TileViewModel> Tiles { get; } = new ObservableCollection<TileViewModel>();

        public ObservableCollection<GameObjectViewModel> Objects { get; } = new ObservableCollection<GameObjectViewModel>();

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

                Tiles.ToList().ForEach(t => t.HandleOverlayChanged());
                Objects.ToList().ForEach(t => t.HandleOverlayChanged());
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
                Objects.ToList().ForEach(t => t.HandleOverlayChanged());
            }
        }

        public void AdvanceTime()
        {
            this.GameWorld = Simulations.simulate(this.GameWorld);
        }
    }
}
