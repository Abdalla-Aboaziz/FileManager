

namespace FileManager.Api.Persistence.EntitesConfigrations
{
    public class UploadedFileConfigration : IEntityTypeConfiguration<UploadedFiles>
    {
        public void Configure(EntityTypeBuilder<UploadedFiles> builder)
        {
            builder.Property(x => x.FileName).HasMaxLength(250);
            builder.Property(x => x.StoredFileName).HasMaxLength(250);
            builder.Property(x => x.ContentType).HasMaxLength(50);
            builder.Property(x => x.FileExtension).HasMaxLength(10);
        }
    }
}
