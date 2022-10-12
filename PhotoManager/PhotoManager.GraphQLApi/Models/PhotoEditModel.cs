using Graph.ArgumentValidator;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.GraphQLApi.Models
{
    [Validatable]
    public class PhotoEditModel
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string DateOfTaking { get; set; }
        [MaxLength(50)]
        public string Place { get; set; }
        public string CameraModel { get; set; }
        public string LensfocalLength { get; set; }
        public string Diaphragm { get; set; }
        public string ShutterSpeed { get; set; }
        public int ISO { get; set; }
        public int Flash { get; set; }
    }
}
