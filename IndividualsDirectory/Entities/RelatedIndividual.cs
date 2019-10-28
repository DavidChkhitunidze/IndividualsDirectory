using IndividualsDirectory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities
{
    public class RelatedIndividual : BaseEntity
    {
        public RelatedIndividualType? RelatedIndividualType { get; set; }

        public int? RelatedToId { get; set; }
        public Individual RelatedTo { get; set; }

        public int? IndividualId { get; set; }
        public Individual Individual { get; set; }
    }
}
