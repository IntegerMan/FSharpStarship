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

        public int TileWidth => View.getImageWidth(World.TileType.Floor);
        public int TileHeight => View.getImageHeight(World.TileType.Floor);

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