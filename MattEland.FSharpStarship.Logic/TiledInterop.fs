namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects
open TiledSharp

module TiledInterop =

  type TiledInfo = 
    {
      Floor: seq<TmxLayerTile>
      Walls: seq<TmxLayerTile>
      Doors: seq<TmxLayerTile>
      Objects: seq<TmxObject>
    }

  let getTilePos (t: TmxLayerTile) = pos t.X t.Y

  let pixelToTile (v: float): int = (int v) / 32

  let getObjectPos (o: TmxObject) = pos (pixelToTile o.X) (pixelToTile o.Y)

  let allTiledObjects (tilemap: TmxMap) = tilemap.ObjectGroups |> Seq.collect(fun g -> g.Objects)

  let objectsOfType typeName tilemap = 
    tilemap
    |> allTiledObjects
    |> Seq.where(fun o -> o.Type = typeName)

  let mapObjectTypeByFunc typeName createFunc (tilemap: TmxMap) =
    tilemap
    |> objectsOfType typeName
    |> Seq.map(createFunc)

  let mapObjectType typeName objectType (tilemap: TmxMap) =
    tilemap
    |> mapObjectTypeByFunc typeName (fun obj -> {ObjectType=objectType; Pos=(getObjectPos obj)})

  let getTilesFromLayer (layerName: string) (tilemap: TmxMap) = 
    let layer = tilemap.Layers.[layerName]
    layer.Tiles |> Seq.filter(fun t -> t.Gid > 0)
    
  let getObjects (tilemap: TiledSharp.TmxMap): List<GameObject> =
    let astronauts = tilemap |> mapObjectType "Astronaut" Astronaut
    let doors = 
      tilemap 
      |> getTilesFromLayer "Doors" 
      |> Seq.map(fun d -> {ObjectType=Door(IsOpen=false, IsHorizontal=false); Pos=getTilePos d})

    Seq.append astronauts doors |> Seq.toList

  let getTiles (tilemap: TiledSharp.TmxMap): List<Tile> =
    let floorTiles = 
      tilemap
      |> getTilesFromLayer "Floor"
      |> Seq.map(fun t -> makeTile TileType.Floor (getTilePos t))

    let wallTiles = 
      tilemap
      |> getTilesFromLayer "Walls"
      |> Seq.map(fun t -> makeTile TileType.Wall (getTilePos t))
    
    Seq.append floorTiles wallTiles |> Seq.toList

  let interpretWorld tilemap =
    {
      Floor = tilemap |> getTilesFromLayer "Floor"
      Walls = tilemap |> getTilesFromLayer "Walls"
      Doors = tilemap |> getTilesFromLayer "Doors"
      Objects = tilemap |> allTiledObjects
    }

  let translateToTile tileType tmxTile =
    tmxTile
    |> getTilePos
    |> makeTile tileType

  let translateToObject (tmxObject: TmxObject) = 
    let objectType =
      match tmxObject.Type with
      | "Astronaut" -> Astronaut
    {ObjectType=objectType; Pos=getObjectPos tmxObject}

  let mergeWith (nextLayer: seq<Tile>) (baseLayer: seq<Tile>) = 
    baseLayer
    |> Seq.filter(fun st -> nextLayer |> Seq.tryFind(fun nt -> nt.Pos = st.Pos) |> Option.isNone)
    |> Seq.append nextLayer

  let createDoor (walls: seq<Tile>) doorTile =
    let pos = doorTile |> getTilePos

    let isHorizontal = 
      walls
      |> Seq.tryFind(fun w -> w.Pos = (pos |> offset 0 -1))
      |> Option.isSome

    {ObjectType=Door(IsOpen=false, IsHorizontal=isHorizontal); Pos=pos}

  let generateWorld data =
    let floors = data.Floor |> Seq.map(fun t -> t |> translateToTile Floor)
    let walls = data.Walls |> Seq.map(fun t -> t |> translateToTile Wall)
    let doors = data.Doors |> Seq.map(fun d -> d |> createDoor walls)

    let tiles = 
      floors 
      |> mergeWith walls 
      |> Seq.toList

    let objects = 
      data.Objects 
      |> Seq.map(translateToObject) 
      |> Seq.append doors
      |> Seq.toList

    create tiles objects

  let loadWorld (filename: string) =
    let tiledFile = new TiledSharp.TmxMap(filename)
    tiledFile 
    |> interpretWorld
    |> generateWorld
