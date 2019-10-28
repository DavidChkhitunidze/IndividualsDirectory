using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities.DbConfigurations
{
    public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
    {
        public void Configure(EntityTypeBuilder<Individual> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.LastName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Gender).IsRequired();
            builder.Property(i => i.PersonalNumber).IsRequired().HasMaxLength(11);
            builder.Property(i => i.BirthDate).IsRequired();
            builder.Property(i => i.ImageUrl).HasMaxLength(250);

            builder.HasOne(i => i.City).WithMany(i => i.Individuals).HasForeignKey(i => i.CityId).OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(i => i.PhoneNumbers).WithOne(i => i.Individual).HasForeignKey(i => i.IndividualId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(i => i.RelatedIndividuals).WithOne(i => i.Individual).HasForeignKey(i => i.IndividualId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            builder.ToTable("Individuals");
        }
    }
}
