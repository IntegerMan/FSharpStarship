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

            brush.Freeze(); // TODO: Introduce a dictionary to store unique / reused brushes
            return brush;
        }

        public static Brush GetSolidColorBrush(Color color)
        {
            Brush brush = new SolidColorBrush(color);

            brush.Freeze(); // TODO: Introduce a dictionary to store unique / reused brushes

            return brush;
        }

        public static Brush GetBrushFromArt(Tiles.TileArt art)
        {
            string pathToCheck = "Images/";
            int index = art.TileFile.LastIndexOf(pathToCheck, StringComparison.InvariantCultureIgnoreCase);

            string resourceFile = art.TileFile.Substring(index + pathToCheck.Length);

            if (!IsSupportedResourceFile(resourceFile))
            {
                throw new NotSupportedException($"The resource file {resourceFile} is not supported but was referenced");
            }

            // TODO: I shouldn't need SpriteInfo anymore
            var fakeSprite = new Sprites.SpriteInfo(resourceFile, 
                                                    Sprites.SpriteLocationType.AbsolutePosition,
                                                    art.X, 
                                                    art.Y, 
                                                    0, 
                                                    0, 
                                                    art.Width, 
                                                    art.Height, 
                                                    art.ZIndex);

            return BrushHelpers.GetBrushFromSpriteInfo(fakeSprite);
        }

        private static bool IsSupportedResourceFile(string resourceFile)
        {
            switch (resourceFile.ToLowerInvariant())
            {
                case "tileset1.png":
                case "tileset2.png":
                case "mid-towna5.png":
                case "mid-townb.png":
                case "mid-townc.png":
                case "mid-townd.png":
                case "tilea5_phc_store.png":
                    return true;
                default:
                    return false;
            }
        }

    }
}