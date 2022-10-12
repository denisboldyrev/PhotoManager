using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoManager.Core.Models;

namespace PhotoManager.Data.Settings
{
    public class PhotoRatingConfiguration : IEntityTypeConfiguration<PhotoRating>
    {
        public void Configure(EntityTypeBuilder<PhotoRating> builder)
        {
            builder.ToTable("PhotoRatings").HasKey(k => new { k.UserId, k.PhotoId });
            builder.Ignore(e => e.Id);
        }
    }
}
