using Microsoft.EntityFrameworkCore;
using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoManager.Data.Seed
{
    public class SeedAppDb
    {
        private readonly ApplicationContext _context;
        public SeedAppDb(ApplicationContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            _context.Database.Migrate();
            List<Album> albums = new();
            List<Photo> photos = new();

            if (!_context.Albums.Any())
            {
                albums.Add(new Album { Title = "Nature", Description = "Nature", UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                albums.Add(new Album { Title = "Animals", Description = "Animals", UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                albums.Add(new Album { Title = "Cars", Description = "Cars", UserId = new Guid("a777bde3-68da-48d3-b0bd-38c880cf0af8") });
                albums.Add(new Album { Title = "Sport Cars", Description = "Sport Cars", UserId = new Guid("a777bde3-68da-48d3-b0bd-38c880cf0af8") });
                albums.Add(new Album { Title = "Portraits", Description = "Portraits", UserId = new Guid("8b98f598-a728-4263-a920-ffc8e3b9f86e") });
                albums.Add(new Album { Title = "Buildings", Description = "Buildings", UserId = new Guid("8b98f598-a728-4263-a920-ffc8e3b9f86e") });
                albums.Add(new Album { Title = "Cities", Description = "Cities", UserId = new Guid("e51c9a88-17fd-4130-8a99-b651b8967cfa") });

                foreach (Album a in albums)
                {
                    _context.Albums.Add(a);
                }
                _context.SaveChanges();
            }
            if (!_context.Photos.Any())
            {
                photos.Add(new Photo { Title = "Cat1", FileName = "f24aa6a4-5e1f-4057-b3b7-90117507c222.jpg", Size = 29610, UploadDT = DateTime.Now, UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                photos.Add(new Photo { Title = "Cat2", FileName = "f69c9278-345f-4fb4-b60d-5ae98ce3b3ec.jpg", Size = 47682, UploadDT = DateTime.Now, UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                photos.Add(new Photo { Title = "Cat3", FileName = "e3312629-3aff-4667-9e91-07882d5d6405.jpg", Size = 111530, UploadDT = DateTime.Now, UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                photos.Add(new Photo { Title = "Cat4", FileName = "8b93fb2d-2fdd-40a0-8d64-5db8cbfa37c1.jpg", Size = 70706, UploadDT = DateTime.Now, UserId = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9") });
                _context.SaveChanges();
            };
        }
    }
}
