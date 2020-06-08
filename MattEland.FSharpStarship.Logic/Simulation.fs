namespace MattEland.FSharpStarship.Logic

open Positions
open World

module Simulations =

  type TileContext = 
    {
      tile: Tile;
      up: Option<Tile>;
      down: Option<Tile>;
      left: Option<Tile>;
      right: Option<Tile>;
    }

  let getContext(world: GameWorld, tile: Tile): TileContext =
    {
      tile=tile;
      up=getTile(world, offset(tile.pos, 0, -1));
      down=getTile(world, offset(tile.pos, 0, 1));
      left=getTile(world, offset(tile.pos, -1, 0));
      right=getTile(world, offset(tile.pos, 1, 0));
    }

  let private simulateTile(tile: Tile, world: GameWorld): Tile = 
    let context = getContext(world, tile)
    // TODO: Need to distribute heat / gasses
    {tile with oxygen=tile.oxygen - 0.1M}

  let simulate(world: GameWorld): GameWorld = { world with tiles=world.tiles |> List.map(fun t -> simulateTile(t, world)) }
