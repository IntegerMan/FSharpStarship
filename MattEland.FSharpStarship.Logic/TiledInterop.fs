namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects
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
    |> mapObjectTypeByFunc typeName (fun obj -> {ObjectType=objectType})

  let getTilesFromLayer (layerName: string) (tilemap: TmxMap) = 
    let layer = tilemap.Layers.[layerName]
    layer.Tiles |> Seq.filter(fun t -> t.Gid > 0) |> Seq.toList
    
  let getObjects (tilemap: TiledSharp.TmxMap): List<GameObject> =
    let astronauts = tilemap |> mapObjectType "Astronaut" Astronaut
    let doors = 
      tilemap 
      |> getTilesFromLayer "Doors" 
      |> List.map(fun d -> {ObjectType=Door(IsOpen=false, IsHorizontal=false)})

    List.append astronauts doors

  let buildArt gid zindex (tilemap: TmxMap): TileArt =
    let tileset =
      tilemap.Tilesets
      |> Seq.find(fun t -> gid >= t.FirstGid && gid <= (t.FirstGid + (t.TileCount.Value - 1)))

    let index = gid - tileset.FirstGid
    let numColumns = tileset.Columns.Value
    let row = index % numColumns
    let column = index / numColumns

    {
      TileFile = tileset.Image.Source
      X = row * tileset.TileWidth
      Y = column * tileset.TileHeight
      Width = tileset.TileWidth
      Height = tileset.TileHeight
      ZIndex = zindex
    }

  let buildTile (tilemap: TmxMap) tileType (tile: TmxLayerTile) =
    let pos = tile |> getTilePos

    let art = tilemap |> buildArt tile.Gid 0
    makeTile tileType [] [art] pos

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
      Objects = tilemap |> allTiledObjects
    }

  let translateToTile (tilemap: TmxMap) (flags: TileFlags) (tmxTile: TmxLayerTile) =

    let art = buildArt tmxTile.Gid 0 tilemap
    tmxTile
    |> getTilePos
    |> makeTile flags [] [art]

  let convertToGameObject (tmxObject: TmxObject) =
    let objectType =
      match tmxObject.Type with
      | "Astronaut" -> Astronaut
    {ObjectType=objectType}    

  let addObjectToTiles (tmxObject: TmxObject) tiles = 
    let object = tmxObject |> convertToGameObject

    let pos = getObjectPos tmxObject
    let tile = tiles |> getTile pos
    let newTile = tile |> addObjectToTile object

    tiles |> replaceTile newTile

  let mergeLayerWithTile (nextLayer: seq<Tile>) (tile: Tile) =
    let otherLayerTile = nextLayer |> Seq.tryFind(fun t -> t.Pos = tile.Pos)

    match otherLayerTile with
    | None -> 
      tile
    | Some otherTile -> 
      {tile with Art=tile.Art @ otherTile.Art; Objects=tile.Objects |> List.append otherTile.Objects}

  let getUniqueTiles (sourceLayer: List<Tile>) (otherLayer: List<Tile>) =
    sourceLayer
    |> List.filter(fun t -> 
      otherLayer 
      |> List.tryFind(fun ot -> ot.Pos = t.Pos) 
      |> Option.isNone)

  let mergeWith (nextLayer: List<Tile>) (baseLayer: List<Tile>) = 

    let newTiles = getUniqueTiles nextLayer baseLayer

    baseLayer
    |> List.map (fun t -> mergeLayerWithTile nextLayer t)
    |> List.append newTiles

  let addDoor (wallsPositions: List<Pos>) (doorTile: Tile) =
    let isHorizontal = 
      wallsPositions
      |> List.tryFind(fun w -> w = (doorTile.Pos |> offset 0 -1))
      |> Option.isSome

    doorTile |> addObjectToTile {ObjectType=Door(IsOpen=false, IsHorizontal=isHorizontal)}

  let buildTileLayers (tilemap: TmxMap) data =
    let walls = data.Walls |> List.map(fun t -> t |> translateToTile tilemap wallFlags)
    [
      data.Floor |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.AirPipes |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.WaterPipes |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Grating |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      data.Decorations |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
      walls
      data.Overlays |> List.map(fun t -> t |> translateToTile tilemap tileFlags)
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

  let objectsInPos pos objects = objects |> List.where(fun o -> o |> getObjectPos = pos)

  let addObjectsToTiles objects tiles =
    tiles |> List.map(fun t -> 
        objects
        |> objectsInPos t.Pos
        |> List.map(convertToGameObject)
        |> List.fold(fun lastTile object -> lastTile |> addObjectToTile object) t
      )

  let generateWorld (tilemap: TmxMap) data  =
    flattenTileLayers tilemap data
    |> addDoorsToTiles data
    |> addObjectsToTiles data.Objects

  let loadWorld (filename: string) =
    let tiledFile = new TiledSharp.TmxMap(filename)
    tiledFile 
    |> interpretWorld
    |> generateWorld tiledFile
