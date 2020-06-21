using System.Windows;
using System.Windows.Media;

namespace MattEland.FSharpStarship.Desktop.Rendering
{
    public class TileRenderer : DrawingVisual
    {
        public TileRenderer()
        {
            using (DrawingContext context = RenderOpen())
            {
                context.DrawRectangle(Brushes.CornflowerBlue, null, new Rect(new Size(32, 32)));
            }
        }
    }
}