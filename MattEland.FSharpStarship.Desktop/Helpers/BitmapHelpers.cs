using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MattEland.FSharpStarship.Desktop.Helpers
{
    public static class BitmapHelpers
    {
        private static readonly IDictionary<string, BitmapSource> _images = new Dictionary<string, BitmapSource>();

        public static BitmapSource GetImage(string imageName)
        {
            var key = imageName.ToUpperInvariant();
            if (_images.ContainsKey(key))
            {
                return _images[key];
            }

            var image = new BitmapImage(new Uri($"pack://application:,,,/Images/{imageName}"));
            image.Freeze();
            _images.Add(key, image);

            return image;
        }

        public static CroppedBitmap BuildCroppedBitmap(this BitmapSource image, Int32Rect rect)
        {
            var croppedImage = new CroppedBitmap(image, rect);
            croppedImage.Freeze();
            return croppedImage;
        }
    }
}