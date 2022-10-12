using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace PhotoManager.BusinessLogic
{
    public enum ExifTags
    {
        Title = 270, //ASCII
        Date = 306, // ASCII
        Camera = 272, // ASCII
        FocalLength = 37386, // rational
        Diaphragm = 37378, // rational
        ShutterSpeed = 37377, // rational?
        ISO = 34855, // u16
        Flash = 37385 // u16
    };

    public class ExifReader
    {
        private readonly ASCIIEncoding _encoder;
        private readonly PropertyItem[] _propItems;

        public ExifReader(Image image)
        {
            _encoder = new ASCIIEncoding();
            _propItems = image.PropertyItems;
        }

        public string GetTitle()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.Title).FirstOrDefault();
            if (property == null)
                return null;
            var result = _encoder.GetString(property.Value).Replace("\0", "");
            return result;
        }

        public string GetDate()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.Date).FirstOrDefault();
            if (property == null)
                return default;
            var data = _encoder.GetString(property.Value);
            if (!DateTime.TryParse(data, out DateTime date))
                return "";
            return date.ToString("d");
        }

        public string GetCameraModel()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.Camera).FirstOrDefault();
            if (property == null)
                return null;
            var cameramodel = _encoder.GetString(property.Value).Replace("\0", "");
            return cameramodel;
        }

        public string GetFocalLength()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.FocalLength).FirstOrDefault();
            if (property == null)
                return default;
            var focal = BitConverter.ToUInt16(property.Value);
            var numerator = BitConverter.ToUInt16(property.Value, 0);
            var denominator = BitConverter.ToUInt16(property.Value, 4);
            var result = numerator / denominator;
            return $"{result}mm";
        }
        public string GetDiaphragm()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.Diaphragm).FirstOrDefault();
            if (property == null)
                return default;
            var diaphragm = BitConverter.ToUInt16(property.Value);
            var numerator = BitConverter.ToUInt16(property.Value, 0);
            var denominator = BitConverter.ToUInt16(property.Value, 4);
            var result = (double)numerator / (double)denominator;
            return $"f/{Math.Pow(1.4142, result):#.##}";
        }
        public string GetShutterSpeed()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.ShutterSpeed).FirstOrDefault();
            if (property == null)
                return default;
            var numerator = BitConverter.ToInt32(property.Value, 0);
            var denominator = BitConverter.ToInt32(property.Value, 4);

            var result = (double)numerator / (double)denominator;
            return $"{1}/{Math.Ceiling(Math.Pow(2, result))}";
        }
        public ushort GetISO()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.ISO).FirstOrDefault();
            if (property == null)
                return default;
            return BitConverter.ToUInt16(property.Value);
        }
        public ushort GetFlash()
        {
            var property = _propItems.Where(p => p.Id == (int)ExifTags.Flash).FirstOrDefault();
            if (property == null)
                return default;
            return BitConverter.ToUInt16(property.Value);
        }
    }
}
