using System;
using System.Windows;
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

        public override int PosX => Tile.pos.x * TileWidth;
        public override int PosY => Tile.pos.y * TileHeight;

        public Sprites.SpriteInfo SpriteInfo => Sprites.getSpriteInfo(Tile.tileType);

        public int ImageWidth => SpriteInfo.width * AppView.zoom;
        public int ImageHeight => SpriteInfo.height * AppView.zoom;
        public int ZIndex => SpriteInfo.zIndex;

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
                var image = new BitmapImage(new Uri($"pack://application:,,,/Images/{SpriteInfo.image}"));
                var croppedImage = new CroppedBitmap(image, new Int32Rect(SpriteInfo.x * SpriteInfo.width, SpriteInfo.y * SpriteInfo.height, SpriteInfo.width, SpriteInfo.height));
                var brush = new ImageBrush(croppedImage)
                {
                    Stretch = Stretch.Uniform
                };

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