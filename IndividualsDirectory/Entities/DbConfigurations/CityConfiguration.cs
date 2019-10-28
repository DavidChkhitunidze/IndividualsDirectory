using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities.DbConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name);

            builder.HasMany(i => i.Individuals).WithOne(i => i.City).HasForeignKey(i => i.CityId).OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Cities");
        }
    }
}
