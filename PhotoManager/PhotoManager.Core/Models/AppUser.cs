using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.Core.Models
{
    public enum UserType
    {
        Regular,
        Paid
    }
    public class AppUser : BaseEntity
    {
        [Required]
        public UserType SubscriptionType { get; set; } = UserType.Regular;

        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public virtual ICollection<PhotoRating> PhotoRatings { get; set; } = new List<PhotoRating>();
    }
}
