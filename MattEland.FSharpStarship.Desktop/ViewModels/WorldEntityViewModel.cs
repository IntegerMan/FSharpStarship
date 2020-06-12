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

        public int TileWidth => SpriteInfo.width * AppView.zoom;
        public int TileHeight => SpriteInfo.height * AppView.zoom;

        public virtual void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }

        public View.AppView AppView => _mainViewModel.AppView;

        public abstract Sprites.SpriteInfo SpriteInfo { get; }

        public int ImageWidth => SpriteInfo.width * AppView.zoom;
        public int ImageHeight => SpriteInfo.height * AppView.zoom;
        public int ZIndex => SpriteInfo.zIndex;

        private readonly MainViewModel _mainViewModel;

        protected WorldEntityViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}