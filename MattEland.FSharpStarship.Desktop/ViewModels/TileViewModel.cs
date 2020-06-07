using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public override string ToolTip => $"{Tile.tileType} ({Tile.pos.x}, {Tile.pos.y})";

        public override int PosX
        {
            get
            {
                // Left walls need to align right
                if (Tile.tileType.Equals(World.TileType.WallLeft))
                {
                    return (Tile.pos.x * TileWidth) + TileWidth - ImageWidth;
                }

                return Tile.pos.x * TileWidth;
            }
        }

        public override int PosY => (Tile.pos.y * TileHeight) - ImageHeight + TileHeight;

        public int ImageWidth => View.getImageWidth(Tile.tileType);
        public int ImageHeight => View.getImageHeight(Tile.tileType);
        public int ZIndex => View.getZIndex(Tile.tileType);

        public Brush OverlayBrush
        {
            get
            {
                if (AppView.overlay == View.CurrentOverlay.None) return Brushes.Transparent;

                // TODO: Introduce a brush factory to store unique / reused brushes
                var color = CalculateColor();
                Brush brush = new SolidColorBrush(color);

                brush.Freeze();

                return brush;
            }
        }

        public override Brush Background
        {
            get
            {
                // TODO: Introduce a brush factory to store unique / reused brushes
                var image = new BitmapImage(new Uri($"pack://application:,,,/Images/{Tile.tileType}.png"));
                var brush = new ImageBrush(image); // {Stretch = Stretch.UniformToFill};

                brush.Freeze();

                return brush;
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, AppView);
            return Color.FromRgb(rgb.r, rgb.g, rgb.b);
        }
    }
}