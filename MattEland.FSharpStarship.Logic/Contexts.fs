namespace MattEland.FSharpStarship.Logic

open World
open Positions
open Tiles

module Contexts =

  type TileContext = 
    {
      Tile: Tile;
      Up: Tile option;
      Down: Tile option;
      Left: Tile option;
      Right: Tile option;
    }

  let getContext(world: GameWorld, tile: Tile): TileContext =
    {
      Tile=tile;
      Up=world |> tryGetTile (tile.Pos |> offset 0 -1);
      Down=world |> tryGetTile (tile.Pos |> offset 0 1);
      Left=world |> tryGetTile (tile.Pos |> offset -1 0);
      Right=world |> tryGetTile (tile.Pos |> offset 1 0);
    }

  let getPotentialNeighbors context = [context.Up; context.Right; context.Down; context.Left]
  let getPresentNeighbors context = context |> getPotentialNeighbors |> List.choose id
