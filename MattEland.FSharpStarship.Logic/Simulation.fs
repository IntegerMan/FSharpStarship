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

  let private getPresentNeighbors(context: TileContext): List<Tile> =
    [
      if context.up.IsSome then yield context.up.Value
      if context.right.IsSome then yield context.right.Value
      if context.down.IsSome then yield context.down.Value
      if context.left.IsSome then yield context.left.Value
    ]

  let shareOxygen(world: GameWorld, tile: Tile, neighbor: Tile): GameWorld =
    let mutable newWorld = world
    if neighbor.oxygen <> tile.oxygen then      
      newWorld <- replaceTile(newWorld, tile.pos, {tile with oxygen=tile.oxygen - 0.1M})
      newWorld <- replaceTile(newWorld, neighbor.pos, {neighbor with oxygen=neighbor.oxygen + 0.1M})

    newWorld

  let private simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    let context = getContext(world, tile)

    let mutable newWorld = world

    for neighbor in getPresentNeighbors(context) do
      newWorld <- shareOxygen(newWorld, getTile(newWorld, tile.pos).Value, neighbor)

    newWorld    

  let simulate(world: GameWorld): GameWorld =
    // Get distinct positions in the world
    let positions = world.tiles |> List.map(fun t -> t.pos) |> List.distinct;

    let mutable newWorld = world

    // TODO: List.iter would be more elegant here
    for pos in positions do
      let tile = getTile(newWorld, pos)
      if tile.IsSome then
        newWorld <- simulateTile(tile.Value, newWorld)

    // Return the final world
    newWorld
