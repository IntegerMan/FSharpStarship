namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects

module TiledInterop =

  let getTilePos (t: TiledSharp.TmxLayerTile) = pos t.X t.Y

  let pixelToTile (v: float): int = (int v) / 32

  let getObjectPos (o: TiledSharp.TmxObject) = pos (pixelToTile o.X) (pixelToTile o.Y)

  let allTiledObjects (tilemap: TiledSharp.TmxMap) = tilemap.ObjectGroups |> Seq.collect(fun g -> g.Objects)

  let objectsOfType typeName tilemap = 
    tilemap
    |> allTiledObjects
    |> Seq.where(fun o -> o.Type = typeName)

  let mapObjectTypeByFunc typeName createFunc (tilemap: TiledSharp.TmxMap) =
    tilemap
    |> objectsOfType typeName
    |> Seq.map(createFunc)

  let mapObjectType typeName objectType (tilemap: TiledSharp.TmxMap) =
    tilemap
    |> mapObjectTypeByFunc typeName (fun obj -> {ObjectType=objectType; Pos=(getObjectPos obj)})

  let getTilesFromLayer (layerName: string) (tilemap: TiledSharp.TmxMap) = 
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

  let loadWorld (filename: string) =
    let tiledFile = new TiledSharp.TmxMap(filename)
    let tiles = tiledFile |> getTiles
    let objects = tiledFile |> getObjects
    create tiles objects
