namespace MattEland.FSharpStarship.Logic

open World
open System
open Utils
open Positions

module Simulations =

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

  let maxAirFlow = 0.1M

  let private getPotentialNeighbors context = [context.Up; context.Right; context.Down; context.Left]
  let private getPresentNeighbors context = context |> getPotentialNeighbors |> List.choose id

  let private shareGas(world: GameWorld, tile: Tile, neighbor: Tile, gas: Gas, delta: decimal): GameWorld =

    let tileCurrentGas = getTileGas gas tile
    let neighborCurrentGas = getTileGas gas neighbor

    if neighborCurrentGas < tileCurrentGas then
      let difference = tileCurrentGas - neighborCurrentGas
      let actualDelta = Math.Min(delta, difference / 2.0M) |> truncateToTwoDecimalPlaces

      // Move the gas into the neighbor tile
      world 
      |> replaceTile tile.Pos (tile |> setTileGas gas (tileCurrentGas - actualDelta))
      |> replaceTile neighbor.Pos (neighbor |> setTileGas gas (neighborCurrentGas + actualDelta))

    else
      world

  let canGasFlowInto tileType gas =
    match tileType with
      | Floor | Space -> true
      | _ -> false

  let private simulateTileGas tile gas world =
    let presentNeighbors = getContext(world, tile) |> getPresentNeighbors

    let currentGas = getTileGas gas tile
    let neighbors = presentNeighbors |> List.filter(fun n -> canGasFlowInto n.TileType gas && getTileGas gas n < currentGas)

    if not neighbors.IsEmpty then
      let delta = maxAirFlow / decimal neighbors.Length

      neighbors |> List.fold(fun newWorld neighbor -> shareGas(newWorld, (newWorld |> getTile tile.Pos), neighbor, gas, delta)) world
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
    let newTile = 
      world 
      |> getTile person.Pos 
      |> convertTileGas humanOxygenIntake Gas.Oxygen Gas.CarbonDioxide
    world |> replaceTile person.Pos newTile

  let private simulateAirScrubber (scrubber: GameObject, world: GameWorld): GameWorld =
    let newTile = 
      world 
      |> getTile scrubber.Pos 
      |> convertTileGas scrubberCO2Intake Gas.CarbonDioxide Gas.Oxygen
    world |> replaceTile scrubber.Pos newTile

  let private simulateObject obj world =
    match obj.ObjectType with
    | Astronaut -> simulatePerson(obj, world)
    | AirScrubber -> simulateAirScrubber(obj, world)

  let private simulateObjects tile world =
    world 
    |> getObjects tile.Pos 
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
    world.Tiles 
    |> List.map(fun t -> t.Pos)
    |> List.distinct 
    |> List.fold(fun newWorld p -> simulateTile(getTile p newWorld, newWorld)) world