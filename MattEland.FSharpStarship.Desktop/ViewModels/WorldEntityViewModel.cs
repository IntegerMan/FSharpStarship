using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public abstract class WorldEntityViewModel : NotifyPropertyChangedBase
    {
        public abstract int PosX { get; }
        public abstract int PosY { get; }
        public abstract string ToolTip { get; }
        public virtual Brush Background => BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);

        public int TileWidth => 32 * AppView.Zoom;
        public int TileHeight => 32 * AppView.Zoom;

        public virtual void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }

        public View.AppView AppView => _mainViewModel.AppView;

        public abstract Sprites.SpriteInfo SpriteInfo { get; }

        public int ImageWidth => TileWidth;
        public int ImageHeight => TileHeight;
        public int ZIndex => SpriteInfo.ZIndex;

        private readonly MainViewModel _mainViewModel;

        protected WorldEntityViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}