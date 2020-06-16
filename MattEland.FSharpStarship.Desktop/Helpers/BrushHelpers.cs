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

            Int32Rect rect;
            Stretch stretch;
            if (sprite.LocationType.Equals(Sprites.SpriteLocationType.Cell))
            {
                var tileWidth = sprite.Width;
                var tileHeight = sprite.Height;
                rect = new Int32Rect(sprite.X * tileWidth, sprite.Y * tileHeight, tileWidth, tileHeight);
                stretch = Stretch.Uniform;
            }
            else
            {
                rect = new Int32Rect(sprite.X, sprite.Y, sprite.Width, sprite.Height);
                stretch = Stretch.Fill;
            }

            var croppedImage = new CroppedBitmap(image, rect);
            var brush = new ImageBrush(croppedImage) { Stretch = stretch };

            brush.Freeze(); // TODO: Introduce a brush factory to store unique / reused brushes
            return brush;
        }
    }
}