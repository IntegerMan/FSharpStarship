using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class TileViewModel : NotifyPropertyChangedBase
    {
        public Common.Tile Tile { get; }

        public TileViewModel(Common.Tile tile)
        {
            Tile = tile;
        }

        public int TileWidth => 32;
        public int TileHeight => 32;

        public string ToolTip => Tile.tileType.ToString();

        public int PosX => Tile.pos.x * TileWidth;
        public int PosY => Tile.pos.y * TileHeight;
    }
}