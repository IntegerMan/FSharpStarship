using System.Windows.Media;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class GameObjectViewModel : WorldEntityViewModel
    {
        public GameObjectViewModel(World.GameObject obj, MainViewModel mainViewModel) : base(mainViewModel)
        {
            GameObject = obj;
        }

        public World.GameObject GameObject { get; }

        public override string ToolTip => $"{GameObject.objectType} ({GameObject.pos.x}, {GameObject.pos.y})";

        public override int PosX => GameObject.pos.x * TileWidth;
        public override int PosY => GameObject.pos.y * TileHeight;

        public override Brush Background => Brushes.Cyan;

    }
}