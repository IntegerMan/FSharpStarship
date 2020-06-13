module CarbonDioxideTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations
open TestHelpers
open MattEland.FSharpStarship.Logic.Tiles
open MattEland.FSharpStarship.Logic.Gasses

[<Fact>]
let ``Share CO2 with two Tiles should share CO2`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two Tiles should not over-share CO2`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two Tiles should not over-receive CO2`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.68M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, neighborTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two Tiles should receive CO2`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, neighborTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with three Tiles should share CO2`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.56M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with three Tiles should receive CO2`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.57M, getGasByPos(finalWorld, neighbor1.Pos, Gas.CarbonDioxide))
    Assert.Equal(0.57M, getGasByPos(finalWorld, neighbor2.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with four Tiles should send CO2`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with four Tiles should receive CO2`` () =
    
    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighbor1: Tile = makeFloorTile {X=0;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor2: Tile = makeFloorTile {X=2;Y=1} {standardGas with CarbonDioxide=0.5M}
    let neighbor3: Tile = makeFloorTile {X=1;Y=0} {standardGas with CarbonDioxide=0.5M}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor1.Pos, Gas.CarbonDioxide))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor2.Pos, Gas.CarbonDioxide))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor3.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``CO2 should not flow into walls`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeWallTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.7M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.CarbonDioxide))

[<Fact>]
let ``CO2 should flow into space`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, originTile.Pos, Gas.CarbonDioxide))

    
[<Fact>]
let ``Space should not retain CO2`` () =

    // Arrange
    let originTile: Tile = makeFloorTile {X=1;Y=1} {standardGas with CarbonDioxide=0.7M}
    let neighborTile: Tile = makeSpaceTile {X=0;Y=1}
    let world: GameWorld = {Objects=[];Tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.Pos, Gas.CarbonDioxide))