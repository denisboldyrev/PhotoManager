using System;

namespace PhotoManager.WebApp.Models
{
    public class PhotoWithLikesVM
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public bool Liked { get; set; }
        public int CountLikes { get; set; }
     
        public PhotoWithLikesVM(Guid id, Guid appUserId, bool liked)
        {
            Id = id;
            AppUserId = appUserId;
            Liked = liked;
        }

    }
}
