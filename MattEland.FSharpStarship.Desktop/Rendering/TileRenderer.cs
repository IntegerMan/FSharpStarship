using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpStarship.Desktop.Helpers;
using MattEland.FSharpStarship.Desktop.ViewModels;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.Rendering
{
    public class TileRenderer : DrawingVisual
    {
        public TileRenderer([NotNull] TileViewModel tile, [NotNull] TileRendererHost host)
        {
            Tile = tile ?? throw new ArgumentNullException(nameof(tile));
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Tile.Renderer = host;
            Render();
        }

        public void Render()
        {
            if (Tile == null) return;

            using var context = RenderOpen();

            var rect = new Rect(new Size(Tile.TileWidth, Tile.TileHeight));
            var tile = Tile.Tile;

            // Add layers
            foreach (var img in tile.Art.Select(a => BrushHelpers.GetImageSourceFromArt(a)))
            {
                context.DrawImage(img, rect);
            }

            // Add objects
            foreach (var img in tile.Objects
                .Select(Sprites.getObjectSpriteInfo)
                .Select(si => BrushHelpers.GetImageSourceFromArt(si, Stretch.Uniform))
                .ToList())
            {
                context.DrawImage(img, rect);
            }

            // Add gas particles if needed
            if (Tile.AppView.Overlay == View.CurrentOverlay.Particles)
            {
                var particles = Tile.BuildParticles();
                foreach (var particle in particles)
                {
                    context.DrawEllipse(particle.Background, null, new Point(particle.PosX, particle.PosY), 1, 1);
                }
            }

            // Add Overlay if needed
            if (Tile.AppView.Overlay != View.CurrentOverlay.None && Tile.AppView.Overlay != View.CurrentOverlay.Particles)
            {
                context.DrawRectangle(Tile.BuildOverlayBrush(), null, rect);
            }
        }

        public TileViewModel Tile { get; }
        public TileRendererHost Host { get; }
    }
}