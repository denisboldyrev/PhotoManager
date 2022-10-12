using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.WebApp.Models
{
    public class PhotoVM
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string FileName { get; set; }
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

        public bool IsLiked { get; set; }
        public IReadOnlyList<PhotoRating> PhotoRatings { get; set; }
    }
}
