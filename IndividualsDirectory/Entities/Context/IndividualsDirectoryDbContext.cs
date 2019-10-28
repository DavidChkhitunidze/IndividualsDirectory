using IndividualsDirectory.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities.Context
{
    public class IndividualsDirectoryDbContext : DbContext
    {
        public IndividualsDirectoryDbContext(DbContextOptions<IndividualsDirectoryDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
#if !DEBUG
                Database.Migrate();
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();

            base.OnModelCreating(modelBuilder);
        }
    }
}
