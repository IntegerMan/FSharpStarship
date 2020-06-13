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

        public override Sprites.SpriteInfo SpriteInfo => Sprites.getTileSpriteInfo(Tile.TileType);

        public override string ToolTip => $"{Tile.TileType}\nPos: ({Tile.Pos.X}, {Tile.Pos.Y})\nOxygen: {Tile.Oxygen}\nCO2: {Tile.CarbonDioxide}\nHeat: {Tile.Heat}\nPressure: {Tile.Pressure}";

        public override int PosX => Tile.Pos.X * TileWidth;
        public override int PosY => Tile.Pos.Y * TileHeight;

        public Brush OverlayBrush
        {
            get
            {
                if (AppView.Overlay == View.CurrentOverlay.None) return Brushes.Transparent;

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
                if (Tile.TileType.Equals(World.TileType.Space)) return Brushes.Transparent;

                return BrushHelpers.GetBrushFromSpriteInfo(SpriteInfo);
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);
            return Color.FromRgb(rgb.R, rgb.G, rgb.B);
        }
    }
}