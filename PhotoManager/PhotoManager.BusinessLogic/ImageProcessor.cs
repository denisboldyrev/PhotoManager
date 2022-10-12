using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PhotoManager.BusinessLogic
{
    public static class ImageProcessor
    {
        public static string GetUniqueFileName(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return Guid.NewGuid().ToString() + ext;
        }

        public static void ResizeAndSaveImage(Image image, Size size, string path, int quality)
        {
            var originalWidth = image.Width;
            var originalHeight = image.Height;
            //how many units are there to make the original length
            var hRatio = (float)originalHeight / size.Height;
            var wRatio = (float)originalWidth / size.Width;
            //get the shorter side
            var ratio = Math.Min(hRatio, wRatio);
            var hScale = Convert.ToInt32(size.Height * ratio);
            var wScale = Convert.ToInt32(size.Width * ratio);
            //start cropping from the center
            var startX = (originalWidth - wScale) / 2;
            var startY = (originalHeight - hScale) / 2;
            //crop the image from the specified location and size
            var sourceRectangle = new Rectangle(startX, startY, wScale, hScale);
            //the future size of the image
            var bitmap = new Bitmap(size.Width, size.Height);
            //fill-in the whole bitmap
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            //generate the new image
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }

            Encoder myEncoder = Encoder.Compression;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo myImageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters myEncoderParameters = new(1);
            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            bitmap.Save(path, myImageCodecInfo, myEncoderParameters);
        }
    }
}
