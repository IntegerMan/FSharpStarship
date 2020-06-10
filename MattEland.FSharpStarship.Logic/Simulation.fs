namespace MattEland.FSharpStarship.Logic

open Positions
open World

module Simulations =

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
      let actualDelta = System.Math.Min(delta, difference / 2.0M)

      newWorld <- replaceTile(newWorld, tile.pos, {tile with oxygen=tile.oxygen - actualDelta})
      newWorld <- replaceTile(newWorld, neighbor.pos, {neighbor with oxygen=neighbor.oxygen + actualDelta})

    newWorld

  let simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    let context = getContext(world, tile)

    let mutable newWorld = world

    let presentNeighbors = getPresentNeighbors(context)
    let neighbors = presentNeighbors |> List.filter(fun n -> n.oxygen < tile.oxygen)

    if not neighbors.IsEmpty then
      let delta = 0.1M / decimal neighbors.Length

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
