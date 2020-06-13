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
      up=world |> getTile (tile.pos |> offset 0 -1);
      down=world |> getTile (tile.pos |> offset 0 1);
      left=world |> getTile (tile.pos |> offset -1 0);
      right=world |> getTile (tile.pos |> offset 1 0);
    }

  let maxAirFlow = 0.1M

  let private getPresentNeighbors(context: TileContext): List<Tile> =
    [
      // TODO: Use Option. methods instead
      if context.up.IsSome then yield context.up.Value
      if context.right.IsSome then yield context.right.Value
      if context.down.IsSome then yield context.down.Value
      if context.left.IsSome then yield context.left.Value
    ]

  let private shareGas(world: GameWorld, tile: Tile, neighbor: Tile, gas: Gas, delta: decimal): GameWorld =

    let tileCurrentGas = getTileGas gas tile
    let neighborCurrentGas = getTileGas gas neighbor

    if neighborCurrentGas < tileCurrentGas then
      let difference = tileCurrentGas - neighborCurrentGas
      let actualDelta = Math.Min(delta, difference / 2.0M) |> truncateToTwoDecimalPlaces

      // Move the gas into the neighbor tile
      world 
      |> replaceTile tile.pos (tile |> setTileGas gas (tileCurrentGas - actualDelta))
      |> replaceTile neighbor.pos (neighbor |> setTileGas gas (neighborCurrentGas + actualDelta))

    else
      world

  let canGasFlowInto tileType gas =
    match tileType with
      | Floor | Space -> true
      | _ -> false

  let private simulateTileGas tile gas world =
    let context = getContext(world, tile)
    let presentNeighbors = getPresentNeighbors(context)

    let currentGas = getTileGas gas tile
    let neighbors = presentNeighbors |> List.filter(fun n -> canGasFlowInto n.tileType gas && getTileGas gas n < currentGas)

    if not neighbors.IsEmpty then
      let delta = maxAirFlow / decimal neighbors.Length

      neighbors |> List.fold(fun newWorld neighbor -> shareGas(newWorld, (newWorld |> getTile tile.pos).Value, neighbor, gas, delta)) world
    else
      world

  let humanOxygenIntake = 0.1M
  let scrubberCO2Intake = 0.1M
  
  let private convertTileGas amount gasSource gasGen tile =
    if tile |> getTileGas gasSource >= amount then
      tile
      |> setTileGas gasSource ((tile |> getTileGas gasSource) - amount)
      |> setTileGas gasGen ((tile |> getTileGas gasGen) + amount)
    else
      tile

  let private simulatePerson (person: GameObject, world: GameWorld): GameWorld =
    let tile = world |> getTile person.pos
    match tile with
    | Some t ->
        let newTile = t |> convertTileGas humanOxygenIntake Gas.Oxygen Gas.CarbonDioxide
        world |> replaceTile person.pos newTile
    | None -> world   

  let private simulateAirScrubber (scrubber: GameObject, world: GameWorld): GameWorld =
    let tile = world |> getTile scrubber.pos
    match tile with
    | Some t ->
        let newTile = t |> convertTileGas scrubberCO2Intake Gas.CarbonDioxide Gas.Oxygen
        world |> replaceTile scrubber.pos newTile
    | None -> world   

  let private simulateObject obj world =
    match obj.objectType with
    | Astronaut -> simulatePerson(obj, world)
    | AirScrubber -> simulateAirScrubber(obj, world)

  let private simulateObjects tile world =
    world 
    |> getObjects tile.pos 
    |> List.fold(fun newWorld obj -> newWorld |> simulateObject obj) world


  let simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    // TODO: This would be a lot more efficient if I could do everything in one pas per tile instead of N where N = num gasses
    world
    |> simulateObjects tile
    |> simulateTileGas tile Gas.Oxygen
    |> simulateTileGas tile Gas.CarbonDioxide
    |> simulateTileGas tile Gas.Electrical
    |> simulateTileGas tile Gas.Heat

  let simulate(world: GameWorld): GameWorld =

    world.tiles 
    |> List.map(fun t -> t.pos)
    |> List.distinct 
    |> List.fold(fun newWorld p -> 
          // TODO: It'd be good to make this match unnecessary
          match getTile p newWorld with // Always get the tile fresh off of the newly updated world
          | Some t -> simulateTile(t, newWorld)
          | None -> newWorld) 
          world