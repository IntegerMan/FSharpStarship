using System.Windows;
using System.Windows.Media;
using MattEland.FSharpStarship.Desktop.ViewModels;

namespace MattEland.FSharpStarship.Desktop.Rendering
{
    public class TileRendererHost : FrameworkElement
    {
        public TileRendererHost()
        {
            Loaded += AddVisualToTree;
            Unloaded += RemoveVisualFromTree;
        }

        private void AddVisualToTree(object sender, RoutedEventArgs e)
        {
            _renderer = new TileRenderer((TileViewModel)DataContext, this);
            AddVisualChild(_renderer);
            AddLogicalChild(_renderer);
        }

        private void RemoveVisualFromTree(object sender, RoutedEventArgs e)
        {
            RemoveLogicalChild(_renderer);
            RemoveVisualChild(_renderer);
            _renderer = null;
        }
        private TileRenderer _renderer;

        protected override Visual GetVisualChild(int index) => _renderer;

        protected override int VisualChildrenCount => 1;

        public void RefreshRender()
        {
            _renderer.Render();
            this.InvalidateVisual();
        }
    }
}