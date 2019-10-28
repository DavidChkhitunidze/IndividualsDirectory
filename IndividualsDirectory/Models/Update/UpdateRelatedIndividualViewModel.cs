using FluentValidation;
using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Models.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Update
{
    public class UpdateRelatedIndividualViewModel
    {
        public int? Id { get; set; }

        public RelatedIndividualType? RelatedIndividualType { get; set; }

        public int? RelatedToId { get; set; }

        public static Func<RelatedIndividual, UpdateRelatedIndividualViewModel> Projection
        => (relatedIndividual) => relatedIndividual == null ? null : new UpdateRelatedIndividualViewModel
        {
            Id = relatedIndividual.Id,
            RelatedIndividualType = relatedIndividual.RelatedIndividualType,
            RelatedToId = relatedIndividual.RelatedToId
        };

        public static Func<UpdateRelatedIndividualViewModel, RelatedIndividual, RelatedIndividual> UpdateProjection
        => (updateRelatedIndividual, relatedIndividual) =>
        {
            if (updateRelatedIndividual == null || relatedIndividual == null)
                return null;

            relatedIndividual.RelatedIndividualType = updateRelatedIndividual.RelatedIndividualType;
            relatedIndividual.RelatedToId = updateRelatedIndividual.RelatedToId;

            return relatedIndividual;
        };
    }

    public class UpdateRelatedIndividualValidator : AbstractValidator<UpdateRelatedIndividualViewModel>
    {
        public UpdateRelatedIndividualValidator()
        {
            RuleFor(i => i.RelatedIndividualType).NotEmpty().IsInEnum();
            RuleFor(i => i.RelatedToId).NotEmpty();
        }
    }
}
