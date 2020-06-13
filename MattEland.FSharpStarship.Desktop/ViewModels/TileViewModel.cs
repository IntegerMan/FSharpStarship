using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : WorldEntityViewModel
    {
        public World.Tile Tile { get; }

        public TileViewModel(World.Tile tile, MainViewModel mainViewModel) : base(mainViewModel)
        {
            Tile = tile;
        }

        public override void HandleOverlayChanged()
        {
            base.HandleOverlayChanged();

            OnPropertyChanged(nameof(OverlayBrush));
        }

        public override Sprites.SpriteInfo SpriteInfo => Sprites.getTileSpriteInfo(Tile.tileType);

        public override string ToolTip => $"{Tile.tileType}\nPos: ({Tile.pos.X}, {Tile.pos.Y})\nOxygen: {Tile.oxygen}\nCO2: {Tile.carbonDioxide}\nHeat: {Tile.heat}";

        public override int PosX => Tile.pos.X * TileWidth;
        public override int PosY => Tile.pos.Y * TileHeight;

        public Brush OverlayBrush
        {
            get
            {
                if (AppView.overlay == View.CurrentOverlay.None) return Brushes.Transparent;

                var color = CalculateColor();
                Brush brush = new SolidColorBrush(color);

                brush.Freeze(); // TODO: Introduce a brush factory to store unique / reused brushes

                return brush;
            }
        }

        public override Brush Background
        {
            get
            {
                if (Tile.tileType.Equals(World.TileType.Space)) return Brushes.Transparent;

                return BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);
            return Color.FromRgb(rgb.r, rgb.g, rgb.b);
        }
    }
}