module AirScrubberTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations
open TestHelpers
open MattEland.FSharpStarship.Logic.GameObjects
open MattEland.FSharpStarship.Logic.Gasses

[<Fact>]
let ``Scrubbers should reduce the amount of CO2`` () =
  // Arrange
  let scrubber: GameObject = {ObjectType=AirScrubber; Pos={X=1;Y=1}}
  let tile = makeFloorTile scrubber.Pos [scrubber] {standardGas with CarbonDioxide=0.7M}
  let world: GameWorld = {Tiles=[tile]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, scrubber.Pos, Gas.CarbonDioxide) < 0.7M)


[<Fact>]
let ``Scrubbers should increase the amount of Oxygen`` () =
  // Arrange
  let scrubber: GameObject = {ObjectType=AirScrubber; Pos={X=1;Y=1}}
  let tile = makeFloorTile scrubber.Pos [scrubber] {standardGas with Oxygen=0.3M; CarbonDioxide=0.1M}
  let world: GameWorld = {Tiles=[tile]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, scrubber.Pos, Gas.Oxygen) > 0.3M)


[<Fact>]
let ``Scrubbers should not produce Oxygen without carbon dioxide`` () =
  // Arrange
  let scrubber: GameObject = {ObjectType=AirScrubber; Pos={X=1;Y=1}}
  let tile = makeFloorTile scrubber.Pos [scrubber] {standardGas with CarbonDioxide=0M; Oxygen=0.3M}
  let world: GameWorld = {Tiles=[tile]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.Equal(0.3M, getGasByPos(newWorld, scrubber.Pos, Gas.Oxygen))