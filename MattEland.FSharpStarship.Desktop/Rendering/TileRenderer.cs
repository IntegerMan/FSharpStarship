using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Desktop.ViewModels;

namespace MattEland.FSharpStarship.Desktop.Rendering
{
    public class TileRenderer : DrawingVisual
    {
        public TileRenderer([NotNull] TileViewModel tile)
        {
            Tile = tile ?? throw new ArgumentNullException(nameof(tile));
            Render();
        }

        private void Render()
        {
            if (Tile == null) return;

            using var context = RenderOpen();

            foreach (var img in Tile.Tile.Art.Select(a => BrushHelpers.GetImageSourceFromArt(a)))
            {
                context.DrawImage(img, new Rect(new Size(Tile.TileWidth, Tile.TileWidth)));
            }
        }

        public TileViewModel Tile { get; }
    }
}