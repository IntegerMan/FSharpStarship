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

  let private shareGas(world: GameWorld, tile: Tile, neighbor: Tile, gas: Gas, delta: decimal): GameWorld =
    let mutable newWorld = world

    let tileCurrentGas = getTileGas(tile, gas)
    let neighborCurrentGas = getTileGas(neighbor, gas)

    if neighborCurrentGas < tileCurrentGas then
      let difference = tileCurrentGas - neighborCurrentGas
      let actualDelta = Math.Min(delta, difference / 2.0M) |> truncateToTwoDecimalPlaces

      // Remove the gas from the source tile
      newWorld <- replaceTile(newWorld, tile.pos, setTileGas(tile, gas, tileCurrentGas - actualDelta))

      // Move the gas into the neighbor tile
      newWorld <- replaceTile(newWorld, neighbor.pos, setTileGas(neighbor, gas, neighborCurrentGas + actualDelta))

    newWorld

  let canGasFlowInto(tileType, gas) =
    match tileType with
      | Floor | Space -> true
      | _ -> false

  let private simulateTileGas tile gas world =
    let mutable newWorld = world
    let context = getContext(world, tile)
    let presentNeighbors = getPresentNeighbors(context)

    let neighbors = presentNeighbors |> List.filter(fun n -> canGasFlowInto(n.tileType, gas) && getTileGas(n, gas) < getTileGas(tile, gas))

    if not neighbors.IsEmpty then
      let delta = maxAirFlow / decimal neighbors.Length

      for neighbor in neighbors do
        newWorld <- shareGas(newWorld, getTile(newWorld, tile.pos).Value, neighbor, gas, delta)

    newWorld    
    

  let simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    world
    |> simulateTileGas tile Gas.Oxygen
    |> simulateTileGas tile Gas.CarbonDioxide
    |> simulateTileGas tile Gas.Electrical
    |> simulateTileGas tile Gas.Heat

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
