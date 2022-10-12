using System;
using System.Collections.Generic;

namespace PhotoManager.WebApp.Models
{
    public class AddPhotosToAlbumsVM
    {
        public List<Guid> Albums { get; set; }
        public List<Guid> Photos { get; set; }
    }
}
