using System.Windows;
using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.ViewModels;

namespace MattEland.FSharpStarship.Desktop.Rendering
{
    public class TileRendererHost : FrameworkElement
    {
        private TileRenderer _renderer;

        protected override Visual GetVisualChild(int index)
        {
            return _renderer ??= new TileRenderer((TileViewModel) DataContext);
        }

        protected override int VisualChildrenCount => 1;
    }
}