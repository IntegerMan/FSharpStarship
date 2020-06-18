namespace MattEland.FSharpStarship.Logic

open Positions
open World
open Tiles
open GameObjects

module TiledInterop =

  let getTilePos (t: TiledSharp.TmxLayerTile) = pos t.X t.Y

  let pixelToTile (v: float): int = (int v) / 32

  let getObjectPos (o: TiledSharp.TmxObject) = pos (pixelToTile o.X) (pixelToTile o.Y)

  let getObjects (tilemap: TiledSharp.TmxMap): List<GameObject> =
    tilemap.ObjectGroups
    |> Seq.collect(fun g -> g.Objects)
    |> Seq.where(fun o -> o.Type = "Astronaut")
    |> Seq.map(fun a -> {ObjectType=Astronaut; Pos=(getObjectPos a)})
    |> Seq.toList

  let getTiles (tilemap: TiledSharp.TmxMap): List<Tile> =
    tilemap.Layers.["Floor"].Tiles
    |> Seq.filter(fun t -> t.Gid > 0)
    |> Seq.map(fun t -> makeTile TileType.Floor (getTilePos t))
    |> Seq.toList

  let loadWorld (filename: string) =
    let tiledFile = new TiledSharp.TmxMap(filename)
    let tiles = tiledFile |> getTiles
    let objects = tiledFile |> getObjects
    create tiles objects
