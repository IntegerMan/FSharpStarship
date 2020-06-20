namespace MattEland.FSharpStarship.Logic

open World
open Tiles
open Gasses
open GameObjects
open SimulateGasses

module Simulations =

  let humanOxygenIntake = 0.1M
  let scrubberCO2Intake = 0.1M
  
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
    | _ -> world

  let private simulateObjects objects world = objects |> List.fold(fun newWorld obj -> newWorld |> simulateObject obj) world

  let simulateTile(tile: Tile, world: GameWorld): GameWorld = 
    world 
    |> simulateObjects tile.Objects 
    |> simulateTileGas tile.Pos

  let simulate(world: GameWorld): GameWorld =
    world.Tiles 
    |> List.map(fun t -> t.Pos)
    |> List.distinct 
    |> List.fold(fun newWorld p -> simulateTile(getTile p newWorld, newWorld)) world