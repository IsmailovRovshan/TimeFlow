using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => new { l.TeacherId, l.ClientId });

            builder.HasOne(l => l.Teacher)
                   .WithMany(t => t.Lessons)
                   .HasForeignKey(l => l.TeacherId);

            builder.HasOne(l => l.Client)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.ClientId);

            builder.Property(l => l.LessonDate)
                   .IsRequired();

            builder.Property(l => l.Status)
                   .IsRequired();
        }
    }
}
