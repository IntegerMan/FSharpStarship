using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.Annotations;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : NotifyPropertyChangedBase
    {
        private readonly MainViewModel _mainViewModel;
        public World.Tile Tile { get; }

        public TileViewModel(World.Tile tile, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Tile = tile;
        }

        public int TileWidth => 32;
        public int TileHeight => 32;

        public string ToolTip => $"{Tile.tileType} ({Tile.pos.x}, {Tile.pos.y})";

        [UsedImplicitly]
        public int PosX => Tile.pos.x * TileWidth;

        [UsedImplicitly]
        public int PosY => Tile.pos.y * TileHeight;

        public Brush Background
        {
            get
            {
                var color = CalculateColor();

                // TODO: Introduce a brush factory to store unique / reused brushes
                Brush brush = new SolidColorBrush(color);
                brush.Freeze();

                return brush;
            }
        }

        private Color CalculateColor()
        {
            var rgb = View.getBackgroundColor(Tile, _mainViewModel.AppView);
            return Color.FromRgb(rgb.r, rgb.g, rgb.b);
        }

        public void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }
    }
}