using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.Core.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public virtual Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
