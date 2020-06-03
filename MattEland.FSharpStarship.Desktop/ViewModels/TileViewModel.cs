using System.Windows.Media;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : NotifyPropertyChangedBase
    {
        private readonly MainViewModel _mainVM;
        public Common.Tile Tile { get; }

        public TileViewModel(Common.Tile tile, MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
            Tile = tile;
        }

        public int TileWidth => 32;
        public int TileHeight => 32;

        public string ToolTip => $"{Tile.tileType} ({Tile.pos.x}, {Tile.pos.y})";

        public int PosX => Tile.pos.x * TileWidth;
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
            if (_mainVM.SelectedViewMode == "Heat")
            {
                return Colors.Salmon;
            }

            var rgb = Styles.getBackgroundColor(Tile); // TODO: It feels like this perhaps should be managed by the F# code, with a current view mode property. Not sure where lines should be drawn.
            var color = Color.FromRgb(rgb.r, rgb.g, rgb.b);
            return color;
        }

        public void HandleOverlayChanged()
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(ToolTip));
        }
    }
}