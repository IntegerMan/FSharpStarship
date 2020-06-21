namespace MattEland.FSharpStarship.Logic

open GameObjects

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

  let getObjectSpriteInfo (object: GameObject): SpriteInfo =
    match object.ObjectType with
    | Astronaut -> { Image="Matt.png"; X=0; Y=0; Width=48; Height=96; ZIndex=5; OffsetY = -2; LocationType = Cell; OffsetX=0}
    | AirScrubber -> { sciWhiteTile with X=7; Y=11}
    | Door (IsHorizontal=true; IsOpen=true) -> {sciWhiteTile with X=6; Y=5; ZIndex=2}
    | Door (IsHorizontal=true; IsOpen=false) -> {sciWhiteTile with X=5; Y=5; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=false) -> {sciWhiteTile with X=4; Y=4; ZIndex=2}
    | Door (IsHorizontal=false; IsOpen=true) -> {sciWhiteTile with X=4; Y=5; ZIndex=2}
