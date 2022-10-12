using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.Core.Models
{
    public class Photo : BaseEntity
    {
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string FileName { get; set; }
        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }
        [Display(Name = "Uploaded")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }
        public string DateOfTaking { get; set; }
        [MaxLength(50)]
        public string Place { get; set; }
        public string CameraModel { get; set; }
        public string LensfocalLength { get; set; }
        public string Diaphragm { get; set; }
        public string ShutterSpeed { get; set; }
        public int ISO { get; set; }
        public int Flash { get; set; }

        public ICollection<Album> Albums { get; set; } = new List<Album>();
        public ICollection<PhotoRating> PhotoRatings { get; set; } = new List<PhotoRating>();
    }
}
