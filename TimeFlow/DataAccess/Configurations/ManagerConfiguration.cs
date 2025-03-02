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
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Login)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(m => m.Password)
                   .IsRequired();

            builder.Property(m => m.Email)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(m => m.FullName)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(m => m.Clients)
                   .WithOne(c => c.Manager)
                   .HasForeignKey(c => c.ManagerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
