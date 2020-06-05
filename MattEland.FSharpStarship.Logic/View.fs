namespace MattEland.FSharpStarship.Logic

open World

module View =

  type CurrentOverlay =
    | None = 0
    | Thermal = 1
    | Oxygen = 2
    | CarbonDioxide = 3
    | Electrical = 4
    | Fluid = 5

  type AppView = {
    overlay: CurrentOverlay
  }

  let getDefaultAppView() = {
    overlay=CurrentOverlay.None; 
  }

  let changeOverlay(view: AppView, newOverlay: CurrentOverlay): AppView = {view with overlay = newOverlay}    

  type RGB = {r: byte; g: byte; b: byte}

  let private rgb (r, g, b): RGB = {r = byte r; g = byte g; b = byte b}

  let private getTileColor (tileType: TileType): RGB =
    match tileType with
    | Floor -> rgb(86, 86, 128)
    | Wall -> rgb(64, 64, 84)
    | _ -> rgb(255, 0, 255) // Magenta for high visibility

  let private getGradedColor(percent: decimal): RGB = 
    let value = (percent * 255M) |> System.Math.Round |> int
    rgb(value, value, value)

  let getBackgroundColor (tile: Tile, view: AppView): RGB =
    match view.overlay with
    | CurrentOverlay.Oxygen -> getGradedColor(tile.oxygen)
    | CurrentOverlay.CarbonDioxide -> getGradedColor(tile.carbonDioxide)
    | CurrentOverlay.Thermal -> getGradedColor(tile.heat)
    | _ -> getTileColor(tile.tileType)
