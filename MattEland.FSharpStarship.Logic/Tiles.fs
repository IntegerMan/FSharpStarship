namespace MattEland.FSharpStarship.Logic

open Positions
open Gasses
open GameObjects

module Tiles =

  type TileFlags =
    {
      RetainsGas: bool
      BlocksGas: bool
      BlocksMovement: bool
      IsTransparent: bool
    }

  type TileArt =
    {
      TileFile: string
      X: int
      Y: int
      Width: int
      Height: int
      ZIndex: int
    }

  let transparentArt =
    {
      TileFile="Transparent"
      X=0
      Y=0
      Width=0
      Height=0
      ZIndex=0
    }
  
  type Tile = 
    {
      Art: List<TileArt> // TODO: Seq
      Flags: TileFlags
      Pos: Pos 
      Pressure: decimal
      Gasses: TileGas
      Objects: List<GameObject> // TODO: Seq
    }

  let spaceFlags = {RetainsGas=false; BlocksGas=false; BlocksMovement=false; IsTransparent=true}
  let wallFlags = {RetainsGas=false; BlocksGas=true; BlocksMovement=true; IsTransparent=false}
  let obstacleFlags = {RetainsGas=true; BlocksGas=false; BlocksMovement=true; IsTransparent=true}
  let tileFlags = {RetainsGas=true; BlocksGas=false; BlocksMovement=false; IsTransparent=false}
  let doorFlags = {RetainsGas=true; BlocksGas=true; BlocksMovement=false; IsTransparent=false}

  let addObject object tile: Tile = {tile with Objects=object::tile.Objects}     
  let addObjects objects tile: Tile = {tile with Objects=objects |> List.append tile.Objects}     
  let removeObject object tile: Tile = {tile with Objects=tile.Objects |> List.except([object])}
  let removeObjects objects tile: Tile = {tile with Objects=tile.Objects |> List.except(objects)}
  
  let replaceObject oldObject newObject tile =
    tile
    |> removeObject oldObject
    |> addObject newObject
    
  let getTilesInRadius pos radius tiles =
    tiles
    |> List.filter(fun t ->
          let distance = getDistance t.Pos pos
          distance <= radius
        )
    
  let getObjectsInRadius tile radius tiles =
    tiles
    |> getTilesInRadius tile.Pos radius
    |> List.collect(fun t -> t.Objects)
       
    