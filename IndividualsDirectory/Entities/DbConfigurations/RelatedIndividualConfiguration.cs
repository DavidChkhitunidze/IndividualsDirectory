using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities.DbConfigurations
{
    public class RelatedIndividualConfiguration : IEntityTypeConfiguration<RelatedIndividual>
    {
        public void Configure(EntityTypeBuilder<RelatedIndividual> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.RelatedIndividualType).IsRequired();

            builder.HasOne(i => i.Individual).WithMany(i => i.RelatedIndividuals).HasForeignKey(i => i.IndividualId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(i => i.RelatedTo).WithMany().HasForeignKey(i => i.RelatedToId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("RelatedIndividuals");
        }
    }
}
