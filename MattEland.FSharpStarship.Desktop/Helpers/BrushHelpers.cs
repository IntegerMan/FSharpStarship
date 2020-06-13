using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.Helpers
{
    public static class BrushHelpers
    {
        public static ImageBrush GetBrushFromSpriteInfo(Sprites.SpriteInfo sprite)
        {
            var image = new BitmapImage(new Uri($"pack://application:,,,/Images/{sprite.Image}"));

            var tileWidth = sprite.Width;
            var tileHeight = sprite.Height;
            var rect = new Int32Rect(sprite.X * tileWidth, sprite.Y * tileHeight, tileWidth, tileHeight);

            var croppedImage = new CroppedBitmap(image, rect);
            var brush = new ImageBrush(croppedImage) { Stretch = Stretch.Uniform };

            brush.Freeze(); // TODO: Introduce a brush factory to store unique / reused brushes
            return brush;
        }
    }
}