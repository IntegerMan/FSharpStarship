using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.Helpers;
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

        public override string ToolTip => $"{GameObject.objectType} ({GameObject.pos.X}, {GameObject.pos.Y})";

        public override int PosX => GameObject.pos.X * TileWidth;
        public override int PosY => GameObject.pos.Y * TileHeight;

        public override Sprites.SpriteInfo SpriteInfo => Sprites.getObjectSpriteInfo(GameObject);
    }
}