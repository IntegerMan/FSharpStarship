module HeatTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Share Heat with two tiles should share heat`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two tiles should not over-share heat`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two tiles should not over-receive heat`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, neighborTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two tiles should receive heat`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, neighborTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with three tiles should share heat`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with three tiles should receive heat`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor1.pos, Gas.Heat))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor2.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with four tiles should send heat`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with heat=0.5M}
    let neighbor3: Tile = {makeTile(TileType.Floor, {X=0;Y=1}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.61M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))

[<Fact>]
let ``Share Heat with four tiles should receive heat`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with heat=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with heat=0.5M}
    let neighbor3: Tile = {makeTile(TileType.Floor, {X=0;Y=1}) with heat=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor1.pos, Gas.Heat))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor2.pos, Gas.Heat))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor3.pos, Gas.Heat))

[<Fact>]
let ``Heat should not flow into walls`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Wall, {X=1;Y=0}) with heat=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.7M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.pos, Gas.Heat))

[<Fact>]
let ``Heat should flow into space`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Space, {X=1;Y=0}) with heat=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.Heat))

    
[<Fact>]
let ``Space should not retain heat`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with heat=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Space, {X=1;Y=0}) with heat=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.pos, Gas.Heat))