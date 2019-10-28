using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.ResourceControls
{
    public class ResourceParams<TViewModel>
    {
        public int Page { get; set; } = 1;

        public int Size { get; set; } = 5;

        public string Search { get; set; }

        public string OrderBy { get; set; } = nameof(BaseEntity.Id).ToLower();

        public string Sort { get; set; } = "desc";

        public TViewModel Filter { get; set; }
    }
}
