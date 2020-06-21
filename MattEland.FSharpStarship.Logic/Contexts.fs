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

  let getContext (tiles: List<Tile>) tile =
    {
      Tile=tile;
      Up=tiles |> tryGetTile (tile.Pos |> offset 0 -1);
      Down=tiles |> tryGetTile (tile.Pos |> offset 0 1);
      Left=tiles |> tryGetTile (tile.Pos |> offset -1 0);
      Right=tiles |> tryGetTile (tile.Pos |> offset 1 0);
    }

  let getPotentialNeighbors context = [context.Up; context.Right; context.Down; context.Left]
  let getPresentNeighbors context = context |> getPotentialNeighbors |> List.choose id
