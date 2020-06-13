module OxygenTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations
open MattEland.FSharpStarship.Logic.Tiles
open MattEland.FSharpStarship.Logic.Gasses
open TestHelpers

[<Fact>]
let ``Share Oxygen with two Tiles should share Oxygen`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with two Tiles should not over-share Oxygen`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with two Tiles should not over-receive Oxygen`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with two Tiles should receive Oxygen`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with three Tiles should share Oxygen`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with three Tiles should receive Oxygen`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor1.Pos, Gas.Oxygen))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor2.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with four Tiles should send Oxygen`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.61M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Share Oxygen with four Tiles should receive Oxygen`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with Oxygen=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with Oxygen=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor1.Pos, Gas.Oxygen))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor2.Pos, Gas.Oxygen))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor3.Pos, Gas.Oxygen))

[<Fact>]
let ``Oxygen should not flow into walls`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeWallTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.7M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Oxygen))

[<Fact>]
let ``Oxygen should flow into space`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.Oxygen))

    
[<Fact>]
let ``Space should not retain Oxygen`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with Oxygen=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.Oxygen))