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

        public override string ToolTip => $"{GameObject.ObjectType} ({GameObject.Pos.X}, {GameObject.Pos.Y})";

        public override int PosX => GameObject.Pos.X * TileWidth;
        public override int PosY => GameObject.Pos.Y * TileHeight;

        public override Sprites.SpriteInfo SpriteInfo => Sprites.getObjectSpriteInfo(GameObject);
    }
}