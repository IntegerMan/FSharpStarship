using System;
using System.Windows;
using System.Windows.Media;
using JetBrains.Annotations;

namespace MattEland.FSharpStarship.Desktop.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public int ZIndex { get; }

        public virtual Brush Background { get; }

        public decimal Opacity { get; }
        public virtual int Width => 32;
        public virtual int Height => 32;
        public virtual int PosX => 0;
        public virtual int PosY => 0;

        public Rect Rectangle => new Rect(new Size(Width, Height));

        public ImageViewModel([NotNull] Brush background, int zIndex, decimal opacity = 1.0M)
        {
            ZIndex = zIndex;
            Background = background ?? throw new ArgumentNullException(nameof(background));
            Opacity = opacity;
        }
    }
}