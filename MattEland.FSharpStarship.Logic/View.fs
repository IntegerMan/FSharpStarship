namespace MattEland.FSharpStarship.Logic

open Tiles

module View =

  type CurrentOverlay =
    | None = 0
    | Heat = 1
    | Nitrogen = 2
    | Oxygen = 3
    | CarbonDioxide = 4
    | Electrical = 5
    | Fluid = 6
    | Pressure = 7

  type AppView = {
    Overlay: CurrentOverlay;
    Zoom: int;
  }

  let getDefaultAppView() = {
    Overlay=CurrentOverlay.None; 
    Zoom=2;
  }

  let changeOverlay(view: AppView, newOverlay: CurrentOverlay): AppView = {view with Overlay = newOverlay}    

  type RGB = {R: byte; G: byte; B: byte}

  let private rgb (r, g, b): RGB = {R = byte r; G = byte g; B = byte b}

  let private getTileColor tileType =
    match tileType with
    | Floor -> rgb(86, 86, 128)
    | Wall -> rgb(64, 64, 84)
    | _ -> rgb(255, 0, 255) // Magenta for high visibility

  let private getGradedColor percent = 
    let value = (System.Math.Min(1M, percent) * 255M) |> System.Math.Round |> int
    rgb(value, value, value)

  let getBackgroundColor (tile: Tile, view: AppView): RGB =
    match view.Overlay with
    | CurrentOverlay.Oxygen -> getGradedColor(tile.Gasses.Oxygen)
    | CurrentOverlay.Nitrogen -> getGradedColor(tile.Gasses.Nitrogen)
    | CurrentOverlay.CarbonDioxide -> getGradedColor(tile.Gasses.CarbonDioxide)
    | CurrentOverlay.Heat -> getGradedColor(tile.Gasses.Heat)
    | CurrentOverlay.Pressure -> getGradedColor(tile.Pressure / 3.0M)
    | _ -> getTileColor(tile.TileType)
