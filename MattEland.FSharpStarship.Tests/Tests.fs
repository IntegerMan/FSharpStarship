module Tests

open System
open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Share Oxygen with two tiles should share oxygen`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = shareOxygen(world, originTile, neighborTile)

    // Assert
    let finalOrigin: Tile = getTile(finalWorld, originTile.pos).Value
    Assert.Equal(0.6M, finalOrigin.oxygen)

[<Fact>]
let ``Share Oxygen with two tiles should receive oxygen`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = shareOxygen(world, originTile, neighborTile)

    // Assert
    let finalNeighbor: Tile = getTile(finalWorld, neighborTile.pos).Value
    Assert.Equal(0.6M, finalNeighbor.oxygen)
