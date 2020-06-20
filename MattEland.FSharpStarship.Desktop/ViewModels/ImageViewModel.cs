using System;
using System.Windows.Media;
using JetBrains.Annotations;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public int ZIndex { get; }

        public Brush Background { get; }

        public decimal Opacity { get; }
        public int Width => 32;
        public int Height => 32;

        public ImageViewModel([NotNull] Brush background, int zIndex, decimal opacity = 1.0M)
        {
            ZIndex = zIndex;
            Background = background ?? throw new ArgumentNullException(nameof(background));
            Opacity = opacity;
        }
    }
}