using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.FSharpStarship.Logic;
using Microsoft.FSharp.Collections;
using TiledSharp;
using static System.Math;

namespace MattEland.FSharpStarship.Desktop.Helpers
{
    public class TiledAdapter
    {
        private const int TileWidth = 32;
        private const int TileHeight = TileWidth;

        public World.GameWorld BuildWorldFromTileMap(string filepath)
        {
            var map = new TmxMap(filepath);

            var tiles = new List<Tiles.Tile>();
            tiles.AddRange(map.Layers.First(l => l.Name == "Floor").Tiles.Where(t => t.Gid > 0).Select(t => World.makeTile(Tiles.TileType.Floor, GetPos(t))));

            var objects = new List<GameObjects.GameObject>();
            objects.AddRange(map.ObjectGroups.First().Objects.Where(o => o.Type == "Astronaut")
               .Select(a => new GameObjects.GameObject(GetPos(a), GameObjects.GameObjectType.Astronaut)));

            return World.create(ListModule.OfSeq(tiles), ListModule.OfSeq(objects));
        }

        private static Positions.Pos GetPos(TmxObject obj)
        {
            return new Positions.Pos((int)Floor(obj.X / TileWidth), (int)Floor(obj.Y / TileHeight));
        }

        private static Positions.Pos GetPos(TmxLayerTile tile)
        {
            return new Positions.Pos(tile.X, tile.Y);
        }
    }
}