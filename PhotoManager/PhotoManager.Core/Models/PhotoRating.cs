using System;

namespace PhotoManager.Core.Models
{
    public class PhotoRating : BaseEntity
    {
        public bool Rating { get; set; }
        public Guid PhotoId { get; set; }
    }
}
