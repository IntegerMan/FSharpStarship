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
            foreach (var img in tile.Art.Where(a => a.Width > 0).Select(a => BrushHelpers.GetImageSourceFromArt(a)))
            {
                context.DrawImage(img, rect);
            }

            // Add objects
            RenderObjects(context);

            // Add gas particles if needed
            if (Tile.AppView.Overlay == View.CurrentOverlay.Particles)
            {
                RenderParticles(context);
            }

            // Add Overlay if needed
            if (Tile.AppView.Overlay != View.CurrentOverlay.None && Tile.AppView.Overlay != View.CurrentOverlay.Particles)
            {
                context.DrawRectangle(Tile.BuildOverlayBrush(), null, rect);
            }
        }

        private void RenderParticles(DrawingContext context)
        {
            var particles = Tile.BuildParticles();
            foreach (var particle in particles)
            {
                context.DrawEllipse(particle.Background, null, new Point(particle.PosX, particle.PosY), 1, 1);
            }
        }

        private void RenderObjects(DrawingContext context)
        {
            var rect = new Rect(new Size(Tile.TileWidth, Tile.TileHeight));
            
            foreach (var img in Tile.Tile.Objects
                .Select(Sprites.getObjectSpriteInfo)
                .Select(si => BrushHelpers.GetImageSourceFromArt(si, Stretch.Uniform)))
            {
                // Some objects have odd aspects we need to account for
                if (img.Height > img.Width)
                {
                    double ratio = img.Width / img.Height;
                    double offset = Tile.ImageWidth / 2.0 * ratio;
                    
                    var location = new Point(offset, 0);
                    var size = new Size(Tile.TileWidth - offset * 2, Tile.TileHeight);
                    
                    context.DrawImage(img, new Rect(location, size));
                }
                else
                {
                    context.DrawImage(img, rect);
                }
            }
        }

        public TileViewModel Tile { get; }
        public TileRendererHost Host { get; }
    }
}