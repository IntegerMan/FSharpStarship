module AstronautTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Humans should reduce the amount of oxygen`` () =
  // Arrange
  let human: GameObject = {objectType=Astronaut; pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.pos) with oxygen = 0.7M}
  let world: GameWorld = {tiles=[tile]; objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, human.pos, Gas.Oxygen) < 0.7M)


[<Fact>]
let ``Humans should increase the amount of carbon dioxide`` () =
  // Arrange
  let human: GameObject = {objectType=Astronaut; pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.pos) with oxygen = 0.1M; carbonDioxide=0.3M}
  let world: GameWorld = {tiles=[tile]; objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, human.pos, Gas.CarbonDioxide) > 0.3M)


[<Fact>]
let ``Humans should not produce carbon dioxide without oxygen`` () =
  // Arrange
  let human: GameObject = {objectType=Astronaut; pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.pos) with oxygen = 0M; carbonDioxide=0.3M}
  let world: GameWorld = {tiles=[tile]; objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.Equal(0.3M, getGasByPos(newWorld, human.pos, Gas.CarbonDioxide))