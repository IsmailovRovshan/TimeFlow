using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(c => c.Manager)
                   .WithMany(m => m.Clients)
                   .HasForeignKey(c => c.ManagerId);

            builder.HasMany(c => c.Lessons)
                   .WithOne(l => l.Client)
                   .HasForeignKey(l => l.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
