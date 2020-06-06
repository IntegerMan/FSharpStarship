using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MattEland.FSharpStarship.Desktop.Annotations;
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

        public override string ToolTip => $"{Tile.tileType} ({Tile.pos.x}, {Tile.pos.y})";

        public override int PosX => Tile.pos.x * TileWidth;
        public override int PosY => Tile.pos.y * TileHeight;

        public override Brush Background
        {
            get
            {
                Brush brush;

                // TODO: Introduce a brush factory to store unique / reused brushes

                if (AppView.overlay == View.CurrentOverlay.None)
                {
                    var image = new BitmapImage(new Uri($"pack://application:,,,/Images/{Tile.tileType}.png"));
                    brush = new ImageBrush(image);
                }
                else
                {

                    var color = CalculateColor();
                    brush = new SolidColorBrush(color);
                }

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