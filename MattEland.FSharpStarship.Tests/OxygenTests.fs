module Tests

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
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getTileOxygen(finalWorld, originTile.pos))

[<Fact>]
let ``Share Oxygen with two tiles should not over-share oxygen`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getTileOxygen(finalWorld, originTile.pos))

[<Fact>]
let ``Share Oxygen with two tiles should not over-receive oxygen`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getTileOxygen(finalWorld, neighborTile.pos))

[<Fact>]
let ``Share Oxygen with two tiles should receive oxygen`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getTileOxygen(finalWorld, neighborTile.pos))

[<Fact>]
let ``Share Oxygen with three tiles should share oxygen`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {x=1;y=2}) with oxygen=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.6M, getTileOxygen(finalWorld, originTile.pos))

[<Fact>]
let ``Share Oxygen with three tiles should receive oxygen`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {x=1;y=1}) with oxygen=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {x=1;y=0}) with oxygen=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {x=1;y=2}) with oxygen=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getTileOxygen(finalWorld, neighbor1.pos))
    Assert.Equal(0.55M, getTileOxygen(finalWorld, neighbor2.pos))
    