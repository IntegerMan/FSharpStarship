namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open Gasses
open GameObjects
open SimulateGasses
open TiledSharp

module TiledInterop =

  type TiledInfo = 
    {
      Floor: List<TmxLayerTile>
      Walls: List<TmxLayerTile>
      WaterPipes: List<TmxLayerTile>
      AirPipes: List<TmxLayerTile>
      Decorations: List<TmxLayerTile>
      Grating: List<TmxLayerTile>
      Overlays: List<TmxLayerTile>
      Doors: List<TmxLayerTile>
      Objects: List<TmxObject>
      Space: List<TmxLayerTile>
      Obstacles: List<TmxLayerTile>
    }

  let getTilePos (t: TmxLayerTile) = pos t.X t.Y

  let pixelToTile (v: float): int = (int v) / 32

  let getObjectPos (o: TmxObject) = pos (pixelToTile o.X) (pixelToTile o.Y)

  let allTiledObjects (tilemap: TmxMap) = tilemap.ObjectGroups |> Seq.collect(fun g -> g.Objects) |> Seq.toList

  let objectsOfType typeName tilemap = 
    tilemap
    |> allTiledObjects
    |> List.where(fun o -> o.Type = typeName)

  let mapObjectTypeByFunc typeName createFunc (tilemap: TmxMap) =
    tilemap
    |> objectsOfType typeName
    |> List.map(createFunc)

  let mapObjectType typeName objectType (tilemap: TmxMap) =
    tilemap
    |> mapObjectTypeByFunc typeName (fun __ -> {ObjectType=objectType})

  let getTilesFromLayer (layerName: string) (tilemap: TmxMap) = 
    tilemap.Layers.[layerName].Tiles
    |> Seq.filter(fun t -> t.Gid > 0)
    |> Seq.toList
    
  let getObjects (tilemap: TmxMap): List<GameObject> =
    let astronauts = tilemap |> mapObjectType "Astronaut" Astronaut
    let doors = 
      tilemap 
      |> getTilesFromLayer "Doors" 
      |> List.map(fun __ -> {ObjectType=Door(IsOpen=false, IsHorizontal=false)})

    List.append astronauts doors

  let findTilesetForGid gid (tilesets: TmxList<TmxTileset>) =
    tilesets |> Seq.find(fun t -> gid >= t.FirstGid && gid <= (t.FirstGid + (t.TileCount.Value - 1)))

  let getArtFromTileset row column (tileset: TmxTileset) =
    {
      TileFile = tileset.Image.Source
      X = row * tileset.TileWidth
      Y = column * tileset.TileHeight
      Width = tileset.TileWidth
      Height = tileset.TileHeight
      ZIndex = 0
    }

  let buildArt gid (tilemap: TmxMap): TileArt =
    let tileset = tilemap.Tilesets |> findTilesetForGid gid
    let index = gid - tileset.FirstGid
    let numColumns = tileset.Columns.Value
    let row = index % numColumns
    let column = index / numColumns
    tileset |> getArtFromTileset row column

  let buildTile (tilemap: TmxMap) tileType (tile: TmxLayerTile) =
    let pos = tile |> getTilePos

    let art = tilemap |> buildArt tile.Gid
    makeTile tileType [art] pos

  let interpretWorld tilemap =
    {
      Floor = tilemap |> getTilesFromLayer "Floor"
      Walls = tilemap |> getTilesFromLayer "Walls"
      Doors = tilemap |> getTilesFromLayer "Doors"
      WaterPipes = tilemap |> getTilesFromLayer "Water"
      AirPipes = tilemap |> getTilesFromLayer "Air"
      Decorations = tilemap |> getTilesFromLayer "Deco"
      Overlays = tilemap |> getTilesFromLayer "Overlays"
      Grating = tilemap |> getTilesFromLayer "Grating"
      Space = tilemap |> getTilesFromLayer "Space"
      Obstacles = tilemap |> getTilesFromLayer "Obstacles"
      Objects = tilemap |> allTiledObjects
    }

  let getArtForTile (tilemap: TmxMap) flags gid =
    match flags.IsTransparent with
    | false -> buildArt gid tilemap
    | true -> transparentArt
  
  let translateToTile (tilemap: TmxMap) (flags: TileFlags) (tmxTile: TmxLayerTile) =
    let art = getArtForTile tilemap flags tmxTile.Gid
    
    tmxTile
    |> getTilePos
    |> makeTile flags [art]

  let convertToGameObject (tmxObject: TmxObject) =
    match tmxObject.Type with
    | "Astronaut" -> Some {ObjectType=Astronaut}
    | "Vent" -> Some {ObjectType=Vent}
    | "AirScrubber" -> Some {ObjectType=AirScrubber}
    | "WaterTank" -> Some {ObjectType=WaterTank}
    | "Plant" -> Some {ObjectType=Plant}
    | "EngineIntake" -> Some {ObjectType=EngineIntake}
    | _ -> None

  let addObjectToTiles (tmxObject: TmxObject) tiles = 
    let object = tmxObject |> convertToGameObject
    
    match object with
    | None ->
      tiles
    | Some obj ->
      let pos = getObjectPos tmxObject
      let tile = tiles |> getTile pos
      let newTile = tile |> addObject obj

      tiles |> replaceTile newTile

  let mergeFlags baseFlags nextFlags: TileFlags =
    {
      BlocksGas = baseFlags.BlocksGas || nextFlags.BlocksGas
      RetainsGas = baseFlags.RetainsGas && nextFlags.RetainsGas
      BlocksMovement = baseFlags.BlocksMovement || nextFlags.BlocksMovement
      IsTransparent = false
    }

  let mergeTiles baseTile nextTile =
    {baseTile with 
      Art=baseTile.Art @ nextTile.Art
      Objects=baseTile.Objects |> List.append nextTile.Objects
      Flags=mergeFlags baseTile.Flags nextTile.Flags
    }

  let mergeLayerWithTile (nextLayer: List<Tile>) (tile: Tile) =
    let otherLayerTile = nextLayer |> List.tryFind(fun t -> t.Pos = tile.Pos)
    match otherLayerTile with
    | None -> tile
    | Some otherTile -> mergeTiles tile otherTile     

  let getUniqueTiles (sourceLayer: List<Tile>) (otherLayer: List<Tile>) =
    sourceLayer
    |> List.filter(fun t -> 
      otherLayer 
      |> List.tryFind(fun ot -> ot.Pos = t.Pos) 
      |> Option.isNone)

  let ensureProperStartingGasses tile: Tile =
    match tile.Flags.BlocksGas || not tile.Flags.RetainsGas || not (gasCanFlowInto tile) with
    | true -> {tile with Gasses={tile.Gasses with CarbonDioxide = 0M; Oxygen=0M; Nitrogen=0M}; Pressure=0M}
    | false -> tile

  let mergeWith (nextLayer: List<Tile>) (baseLayer: List<Tile>) = 

    let newTiles = getUniqueTiles nextLayer baseLayer

    baseLayer
    |> List.map(fun t -> mergeLayerWithTile nextLayer t)
    |> List.append newTiles
    |> List.map(ensureProperStartingGasses)

  let addDoor (wallsPositions: List<Pos>) (doorTile: Tile) =
    let isHorizontal = 
      wallsPositions
      |> List.tryFind(fun w -> w = (doorTile.Pos |> offset 0 -1))
      |> Option.isSome

    doorTile |> addObject {ObjectType=Door(IsOpen=false, IsHorizontal=isHorizontal)}

  let addAirPipe tile =
    let pipe = {ObjectType=AirPipe emptyGasses } // TODO: defaultGasses
    tile |> addObject pipe
  
  let buildTileLayers (tilemap: TmxMap) data =
    [
      data.Space |> List.map(fun t -> t |> translateToTile tilemap spaceFlags)
      data.Floor |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.AirPipes |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.WaterPipes |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Grating |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Decorations |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Walls |> List.map(fun t -> t |> translateToTile tilemap wallFlags)
      data.Overlays |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Obstacles |> List.map(fun t -> t |> translateToTile tilemap obstacleFlags)
    ]

  let flattenTileLayers (tilemap: TmxMap) data =
    buildTileLayers tilemap data
    |> List.reduce(fun tilesBase tilesNew -> tilesBase |> mergeWith tilesNew)

  let getTileAtPos pos tiles = tiles |> List.find(fun t -> t.Pos = pos)

  let addDoorsToTiles data tiles =
    let wallPositions = data.Walls |> List.map(getTilePos)
    let doors = 
      data.Doors 
      |> List.map(fun d -> d |> getTilePos)
      |> List.map(fun p -> tiles |> getTileAtPos p) 
      |> List.map(fun t -> t |> addDoor wallPositions)

    tiles |> mergeWith doors
        
  let addPipesToTiles data tiles =
    let airPipes =
      data.AirPipes
      |> List.map(getTilePos)
      |> List.map(fun p -> tiles |> getTileAtPos p)
      |> List.map(fun t -> t |> addAirPipe)
      
    tiles |> mergeWith airPipes    

  let objectsInPos pos objects = objects |> List.where(fun o -> o |> getObjectPos = pos)

  let addObjectsToTiles objects tiles =
    tiles |> List.map(fun t -> 
        objects
        |> objectsInPos t.Pos
        |> List.map(convertToGameObject)
        |> List.choose(fun o -> o)
        |> List.fold(fun lastTile object -> lastTile |> addObject object) t
      )

  let generateWorld (tilemap: TmxMap) data  =
    flattenTileLayers tilemap data
    |> addDoorsToTiles data
    |> addPipesToTiles data
    |> addObjectsToTiles data.Objects

  let loadWorld (filename: string) =
    let tiledFile = TmxMap(filename)
    tiledFile 
    |> interpretWorld
    |> generateWorld tiledFile
