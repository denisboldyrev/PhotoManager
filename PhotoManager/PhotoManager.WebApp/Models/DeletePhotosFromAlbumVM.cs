using System;
using System.Collections.Generic;

namespace PhotoManager.WebApp.Models
{
    public class DeletePhotosFromAlbumVM
    {
        public Guid AlbumId { get; set; }
        public List<Guid> PhotosId { get; set; }
    }
}
