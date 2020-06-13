module CarbonDioxideTests

open Xunit
open MattEland.FSharpStarship.Logic.World
open MattEland.FSharpStarship.Logic.Simulations

[<Fact>]
let ``Share CO2 with two tiles should share CO2`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two tiles should not over-share CO2`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two tiles should not over-receive CO2`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.68M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.69M, getGasByPos(finalWorld, neighborTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with two tiles should receive CO2`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, neighborTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with three tiles should share CO2`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with three tiles should receive CO2`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor1.pos, Gas.CarbonDioxide))
    Assert.Equal(0.55M, getGasByPos(finalWorld, neighbor2.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with four tiles should send CO2`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with carbonDioxide=0.5M}
    let neighbor3: Tile = {makeTile(TileType.Floor, {X=0;Y=1}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.61M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``Share CO2 with four tiles should receive CO2`` () =
    
    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighbor1: Tile = {makeTile(TileType.Floor, {X=1;Y=0}) with carbonDioxide=0.5M}
    let neighbor2: Tile = {makeTile(TileType.Floor, {X=1;Y=2}) with carbonDioxide=0.5M}
    let neighbor3: Tile = {makeTile(TileType.Floor, {X=0;Y=1}) with carbonDioxide=0.5M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighbor1; neighbor2; neighbor3]}
    
    // Act
    let finalWorld = simulateTile(originTile, world)
    
    // Assert
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor1.pos, Gas.CarbonDioxide))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor2.pos, Gas.CarbonDioxide))
    Assert.Equal(0.53M, getGasByPos(finalWorld, neighbor3.pos, Gas.CarbonDioxide))

[<Fact>]
let ``CO2 should not flow into walls`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Wall, {X=1;Y=0}) with carbonDioxide=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.7M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.pos, Gas.CarbonDioxide))

[<Fact>]
let ``CO2 should flow into space`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Space, {X=1;Y=0}) with carbonDioxide=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0.6M, getGasByPos(finalWorld, originTile.pos, Gas.CarbonDioxide))

    
[<Fact>]
let ``Space should not retain CO2`` () =

    // Arrange
    let originTile: Tile = {makeTile(TileType.Floor, {X=1;Y=1}) with carbonDioxide=0.7M}
    let neighborTile: Tile = {makeTile(TileType.Space, {X=1;Y=0}) with carbonDioxide=0M}
    let world: GameWorld = {objects=[];tiles=[originTile; neighborTile]}

    // Act
    let finalWorld = simulateTile(originTile, world)

    // Assert
    Assert.Equal(0M, getGasByPos(finalWorld, neighborTile.pos, Gas.CarbonDioxide))