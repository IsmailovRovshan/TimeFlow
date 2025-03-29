using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Login)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(t => t.Password)
                   .IsRequired();

            builder.Property(t => t.Email)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.FullName)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(t => t.Lessons)
               .WithOne(l => l.Teacher)
               .HasForeignKey(l => l.TeacherId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId);
        }
    }
}
