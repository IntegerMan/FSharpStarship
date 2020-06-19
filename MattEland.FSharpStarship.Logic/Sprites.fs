namespace MattEland.FSharpStarship.Logic

open GameObjects
open Tiles

module Sprites =

  type SpriteLocationType =
    | Cell
    | AbsolutePosition

  type SpriteInfo = {
    Image: string
    LocationType: SpriteLocationType
    X: int
    Y: int
    OffsetX: int
    OffsetY: int
    Width: int
    Height: int
    ZIndex: int
  }

  let oryxTile = {
    Image="Oryx.png"
    X=0
    Y=0
    LocationType=Cell
    Width=24
    Height=24
    OffsetX=0
    OffsetY=0
    ZIndex=0
  }

  let sciWhiteTile = {
    Image="tileset2.png"
    X=0
    Y=0
    LocationType=Cell
    Width=32
    Height=32
    OffsetX=0
    OffsetY=0
    ZIndex=0
  }

  let hospitalTile = {
    Image="Hospital.png"
    X=0
    Y=0
    LocationType=AbsolutePosition
    Width=32
    Height=32
    OffsetX=0
    OffsetY=0
    ZIndex=0
  }

  let midTileB = {
    Image="Mid-TownB.png"
    X=0
    Y=0
    LocationType=AbsolutePosition
    Width=32
    Height=32
    OffsetX=0
    OffsetY=0
    ZIndex=0
  }

  let midTileD = {midTileB with Image="Mid-TownD.png"}

  let getTileSpriteInfo (tileType: TileType): SpriteInfo =
    match tileType with
    | Wall -> {sciWhiteTile with X=6; Y=0; ZIndex=1}
    | Floor -> {sciWhiteTile with X=0; Y=11}
    | Carpet -> {hospitalTile with X=241; Y=96; Width=48; Height=48}
    | _ -> sciWhiteTile

  let getObjectSpriteInfo (object: GameObject): SpriteInfo =
    match object.ObjectType with
    | Astronaut -> { Image="Matt.png"; X=0; Y=0; Width=48; Height=96; ZIndex=5; OffsetY = -2; LocationType = Cell; OffsetX=0}
    | AirScrubber -> { sciWhiteTile with X=7; Y=11}
    | Bed (IsLeft=true) -> {midTileB with X=39; Y=79; Width=20; Height=28}
    | Bed (IsLeft=false) -> {midTileB with X=60; Y=79; Width=20; Height=28}
    | Desk (IsLeft=true) -> {midTileD with X=98; Y=226; Width=30; Height=30}
    | Desk (IsLeft=false) -> {midTileD with X=98; Y=196; Width=30; Height=30}
    | SideTable -> {midTileB with X=112; Y=85; Width=16; Height=20}
    | Plant -> {midTileB with X=2; Y=102; Width=30; Height=36}
    | BookShelf (IsLeft=true) -> {midTileB with X=33; Y=214; Width=16; Height=36}
    | BookShelf (IsLeft=false) -> {midTileB with X=47; Y=214; Width=16; Height=36}
    | Shelf (IsLeft=true) -> {hospitalTile with X=575; Y=917; Width=72; Height=108}
    | Shelf (IsLeft=false) -> {hospitalTile with X=647; Y=917; Width=72; Height=108}
    | Door (IsHorizontal=true; IsOpen=true) -> {sciWhiteTile with X=6; Y=5; ZIndex=2}
    | Door (IsHorizontal=true; IsOpen=false) -> {sciWhiteTile with X=5; Y=5; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=false) -> {sciWhiteTile with X=4; Y=4; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=true) -> {sciWhiteTile with X=4; Y=5; ZIndex=2}
    
  let getSpriteInfoFromArt (art: TileArt option): SpriteInfo =
    match art with
      | None -> sciWhiteTile
      | Some a -> midTileB // TODO: Not this