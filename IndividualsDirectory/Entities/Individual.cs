using IndividualsDirectory.Common;
using System;
using System.Collections.Generic;

namespace IndividualsDirectory.Entities
{
    public class Individual : BaseEntity
    {
        public Individual()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();
            RelatedIndividuals = new HashSet<RelatedIndividual>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ImageUrl { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public ICollection<RelatedIndividual> RelatedIndividuals { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
