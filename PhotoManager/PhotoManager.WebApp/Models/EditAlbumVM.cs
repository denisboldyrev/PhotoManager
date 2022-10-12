using Microsoft.AspNetCore.Mvc;
using PhotoManager.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.WebApp.Models
{
    public class EditAlbumVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The Title Field is Required")]
        [RegularExpression(@"^[a-zA-Z0-9\-'_\s]{1,15}$",
          ErrorMessage = "Only A-Z, a-z, 0-9, ' ', '-', '_' characters are allowed.")]
        [MaxLength(15, ErrorMessage = "Title should be up to 15 characters.")]
        [ValidationUniqueTitle]
        [Remote(action: "CheckTitle", controller: "Album")]
        public string Title { get; set; } // must be unique
        [MaxLength(200, ErrorMessage = "Description should be up to 200 characters.")]
        public string Description { get; set; }
        public string Cover { get; set; }

    }
}
