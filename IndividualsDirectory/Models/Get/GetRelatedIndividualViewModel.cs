using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Get
{
    public class GetRelatedIndividualViewModel
    {
        public int Id { get; set; }

        public RelatedIndividualType? RelatedIndividualType { get; set; }

        public GetIndividualPreviewViewModel RelatedTo { get; set; }

        public static Expression<Func<RelatedIndividual, GetRelatedIndividualViewModel>> Projection
        => (relatedPerson) => relatedPerson == null ? null : new GetRelatedIndividualViewModel
        {
            Id = relatedPerson.Id,
            RelatedIndividualType = relatedPerson.RelatedIndividualType,
            RelatedTo = relatedPerson.RelatedTo == null ? null : GetIndividualPreviewViewModel.Projection.Compile().Invoke(relatedPerson.RelatedTo)
        };
    }
}
