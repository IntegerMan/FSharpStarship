namespace MattEland.FSharpStarship.Logic

open World
open System
open Utils
open Positions

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

  let maxAirFlow = 0.1M

  let private getPresentNeighbors(context: TileContext): List<Tile> =
    [
      if context.up.IsSome then yield context.up.Value
      if context.right.IsSome then yield context.right.Value
      if context.down.IsSome then yield context.down.Value
      if context.left.IsSome then yield context.left.Value
    ]

  let private shareOxygen(world: GameWorld, tile: Tile, neighbor: Tile, delta: decimal): GameWorld =
    let mutable newWorld = world
    if neighbor.oxygen < tile.oxygen then

      let difference = tile.oxygen - neighbor.oxygen
      let actualDelta = Math.Min(delta, difference / 2.0M) |> truncateToTwoDecimalPlaces

      // Remove the oxygen from the source tile
      newWorld <- replaceTile(newWorld, tile.pos, {tile with oxygen=tile.oxygen - actualDelta})

      // Move the oxygen into the neighbor tile, unless that tile is space, in which case it is discarded
      if (neighbor.tileType <> TileType.Space) then
        newWorld <- replaceTile(newWorld, neighbor.pos, {neighbor with oxygen=neighbor.oxygen + actualDelta})

    newWorld

  let canOxygenFlowInto tileType =
    match tileType with
      | Floor | Space -> true
      | _ -> false

  let simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    let context = getContext(world, tile)

    let mutable newWorld = world

    let presentNeighbors = getPresentNeighbors(context)
    let neighbors = presentNeighbors |> List.filter(fun n -> canOxygenFlowInto(n.tileType) && n.oxygen < tile.oxygen)

    if not neighbors.IsEmpty then
      let delta = maxAirFlow / decimal neighbors.Length

      for neighbor in neighbors do
        newWorld <- shareOxygen(newWorld, getTile(newWorld, tile.pos).Value, neighbor, delta)

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
