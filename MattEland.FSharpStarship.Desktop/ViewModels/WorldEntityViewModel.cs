using System.Windows.Media;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public abstract class WorldEntityViewModel : NotifyPropertyChangedBase
    {
        public abstract int PosX { get; }
        public abstract int PosY { get; }
        public abstract string ToolTip { get; }
        public abstract Brush Background { get; }

        public int TileWidth => Sprites.getSpriteInfo(World.TileType.Floor).width * AppView.zoom;
        public int TileHeight => Sprites.getSpriteInfo(World.TileType.Floor).height * AppView.zoom;

        public virtual void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }

        public View.AppView AppView => _mainViewModel.AppView;

        private readonly MainViewModel _mainViewModel;

        protected WorldEntityViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}