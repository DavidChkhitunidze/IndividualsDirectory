using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities.DbConfigurations
{
    public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Number).IsRequired().HasMaxLength(50);
            builder.Property(i => i.PhoneNumberType).IsRequired();

            builder.HasOne(i => i.Individual).WithMany(i => i.PhoneNumbers).HasForeignKey(i => i.IndividualId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("PhoneNumbers");
        }
    }
}
