using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MattEland.FSharpStarship.Logic;

namespace MattEland.FSharpStarship.Desktop.Helpers
{
    public static class BrushHelpers
    {
        private static readonly IDictionary<Color, SolidColorBrush> _colorBrushes = new Dictionary<Color, SolidColorBrush>();
        private static readonly IDictionary<string, Brush> _imageRects = new Dictionary<string, Brush>();

        public static Brush GetBrushFromSpriteSheet(string imagePath, int x, int y, int width, int height, Stretch stretch = Stretch.Fill)
        {
            var rect = stretch == Stretch.Uniform 
                ? new Int32Rect(x * width, y * height, width, height) 
                : new Int32Rect(x, y, width, height);

            var image = BitmapHelpers.GetImage(imagePath);
            return image.GetBrushFromImageRect(imagePath, rect, stretch);
        }

        public static ImageSource GetImageSourceFromSpriteSheet(string imagePath, int x, int y, int width, int height, Stretch stretch = Stretch.Fill)
        {
            var rect = stretch == Stretch.Uniform 
                ? new Int32Rect(x * width, y * height, width, height) 
                : new Int32Rect(x, y, width, height);

            var image = BitmapHelpers.GetImage(imagePath);
            return image.GetImageSourceFromImageRect(imagePath, rect, stretch);
        }

        public static Brush GetBrushFromImageRect(this BitmapSource image, string imageKey, Int32Rect rect, Stretch stretch = Stretch.Fill)
        {
            var key = $"brush_{imageKey}:{rect.X},{rect.Y}:{rect.Width},{rect.Height}";

            if (_imageRects.ContainsKey(key))
            {
                return _imageRects[key];
            }

            var brush = new ImageBrush(image.BuildCroppedBitmap(rect)) {Stretch = stretch};
            brush.Freeze();

            _imageRects.Add(key, brush);

            return brush;
        }

        public static ImageSource GetImageSourceFromImageRect(this BitmapSource image, string imageKey, Int32Rect rect, Stretch stretch = Stretch.Fill)
        {
            return image.BuildCroppedBitmap(rect);
        }

        public static SolidColorBrush GetSolidColorBrush(Color color)
        {
            if (_colorBrushes.ContainsKey(color))
            {
                return _colorBrushes[color];
            }

            var brush = new SolidColorBrush(color);
            brush.Freeze();

            _colorBrushes.Add(color, brush);

            return brush;
        }

        public static ImageSource GetImageSourceFromArt(Tiles.TileArt art, Stretch stretch = Stretch.Fill)
        {
            string pathToCheck = "Images/";
            int index = art.TileFile.LastIndexOf(pathToCheck, StringComparison.InvariantCultureIgnoreCase);

            string resourceFile = art.TileFile.Substring(index + pathToCheck.Length);

            return GetImageSourceFromSpriteSheet(resourceFile, art.X, art.Y, art.Width, art.Height, stretch);
        }
    }
}