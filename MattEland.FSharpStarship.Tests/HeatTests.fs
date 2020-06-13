module HeatTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations
open TestHelpers
open MattEland.FSharpStarship.Logic.Tiles
open MattEland.FSharpStarship.Logic.Gasses

[<Fact>]
let ``Share Heat with two Tiles should share heat`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two Tiles should not over-share heat`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two Tiles should not over-receive heat`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with two Tiles should receive heat`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with three Tiles should share heat`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with three Tiles should receive heat`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor1.Pos, Gas.Heat))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor2.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with four Tiles should send heat`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Heat=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.61M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))

[<Fact>]
let ``Share Heat with four Tiles should receive heat`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Heat=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Heat=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with Heat=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor1.Pos, Gas.Heat))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor2.Pos, Gas.Heat))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor3.Pos, Gas.Heat))

[<Fact>]
let ``Heat should not flow into walls`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeWallTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.7M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Heat))

[<Fact>]
let ``Heat should flow into space`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Heat))

    
[<Fact>]
let ``Space should not retain heat`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Heat=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Heat))