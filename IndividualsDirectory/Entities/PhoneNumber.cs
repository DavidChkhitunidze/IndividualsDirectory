using IndividualsDirectory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Entities
{
    public class PhoneNumber : BaseEntity
    {
        public string Number { get; set; }

        public PhoneNumberType? PhoneNumberType { get; set; }

        public int? IndividualId { get; set; }
        public Individual Individual { get; set; }
    }
}
