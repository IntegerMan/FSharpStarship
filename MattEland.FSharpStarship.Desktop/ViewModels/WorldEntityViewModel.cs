using System;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public abstract class WorldEntityViewModel : ViewModelBase
    {
        public abstract int PosX { get; }
        public abstract int PosY { get; }
        public abstract string ToolTip { get; }
        public virtual Brush Background => BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);

        public int TileWidth => 32;
        public int TileHeight => 32;

        public virtual void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }

        public View.AppView AppView => MainVM.AppView;

        public abstract Sprites.SpriteInfo SpriteInfo { get; }

        public int ImageWidth => TileWidth;
        public int ImageHeight => TileHeight;
        public int ZIndex => SpriteInfo.ZIndex;

        protected WorldEntityViewModel([NotNull] MainViewModel mainViewModel)
        {
            MainVM = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public MainViewModel MainVM { get; }
    }
}