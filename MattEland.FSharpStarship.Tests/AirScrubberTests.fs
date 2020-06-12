module AirScrubberTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Scrubbers should reduce the amount of CO2`` () =
  // Arrange
  let obj: GameObject = {objectType=AirScrubber; pos={x=1;y=1}}
  let tile = {makeTile(TileType.Floor, obj.pos) with carbonDioxide = 0.7M}
  let world: GameWorld = {tiles=[tile]; objects=[obj]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, obj.pos, Gas.CarbonDioxide) < 0.7M)


[<Fact>]
let ``Scrubbers should increase the amount of Oxygen`` () =
  // Arrange
  let scrubber: GameObject = {objectType=AirScrubber; pos={x=1;y=1}}
  let tile = {makeTile(TileType.Floor, scrubber.pos) with oxygen = 0.3M; carbonDioxide=0.1M}
  let world: GameWorld = {tiles=[tile]; objects=[scrubber]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, scrubber.pos, Gas.Oxygen) > 0.3M)


[<Fact>]
let ``Scrubbers should not produce oxygen without carbon dioxide`` () =
  // Arrange
  let scrubber: GameObject = {objectType=AirScrubber; pos={x=1;y=1}}
  let tile = {makeTile(TileType.Floor, scrubber.pos) with carbonDioxide = 0M; oxygen=0.3M}
  let world: GameWorld = {tiles=[tile]; objects=[scrubber]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.Equal(0.3M, getGasByPos(newWorld, scrubber.pos, Gas.Oxygen))