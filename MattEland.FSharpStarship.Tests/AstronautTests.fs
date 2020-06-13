module AstronautTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Humans should reduce the amount of Oxygen`` () =
  // Arrange
  let human: GameObject = {ObjectType=Astronaut; Pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.Pos) with Oxygen = 0.7M}
  let world: GameWorld = {Tiles=[tile]; Objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, human.Pos, Gas.Oxygen) < 0.7M)


[<Fact>]
let ``Humans should increase the amount of carbon dioxide`` () =
  // Arrange
  let human: GameObject = {ObjectType=Astronaut; Pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.Pos) with Oxygen = 0.1M; CarbonDioxide=0.3M}
  let world: GameWorld = {Tiles=[tile]; Objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.True(getGasByPos(newWorld, human.Pos, Gas.CarbonDioxide) > 0.3M)


[<Fact>]
let ``Humans should not produce carbon dioxide without Oxygen`` () =
  // Arrange
  let human: GameObject = {ObjectType=Astronaut; Pos={X=1;Y=1}}
  let tile = {makeTile(TileType.Floor, human.Pos) with Oxygen = 0M; CarbonDioxide=0.3M}
  let world: GameWorld = {Tiles=[tile]; Objects=[human]}

  // Act
  let newWorld = simulateTile(tile, world)

  // Assert
  Assert.Equal(0.3M, getGasByPos(newWorld, human.Pos, Gas.CarbonDioxide))