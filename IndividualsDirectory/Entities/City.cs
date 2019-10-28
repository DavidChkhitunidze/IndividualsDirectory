using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities
{
    public class City : BaseEntity
    {
        public City()
        {
            Individuals = new HashSet<Individual>();
        }

        public string Name { get; set; }

        public ICollection<Individual> Individuals { get; set; }
    }
}
