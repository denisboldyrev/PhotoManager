using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.WebApp.Models
{
    public class SetAlbumCoverVM
    {
        public Guid AlbumId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
