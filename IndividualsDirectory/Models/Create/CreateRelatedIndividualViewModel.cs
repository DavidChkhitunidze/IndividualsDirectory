using FluentValidation;
using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Create
{
    public class CreateRelatedIndividualViewModel
    {
        public RelatedIndividualType? RelatedIndividualType { get; set; }

        public int? RelatedToId { get; set; }

        public static Func<CreateRelatedIndividualViewModel, RelatedIndividual> Projection
        => createRelatedIndividual => createRelatedIndividual == null ? null : new RelatedIndividual
        {
            RelatedIndividualType = createRelatedIndividual.RelatedIndividualType,
            RelatedToId = createRelatedIndividual.RelatedToId
        };

        public static implicit operator CreateRelatedIndividualViewModel(UpdateRelatedIndividualViewModel updateRelatedIndividual)
        => new CreateRelatedIndividualViewModel
        {
            RelatedIndividualType = updateRelatedIndividual.RelatedIndividualType,
            RelatedToId = updateRelatedIndividual.RelatedToId
        };
    }

    public class CreateRelatedIndividualValidator : AbstractValidator<CreateRelatedIndividualViewModel>
    {
        public CreateRelatedIndividualValidator()
        {
            RuleFor(i => i.RelatedIndividualType).NotEmpty().IsInEnum();
            RuleFor(i => i.RelatedToId).NotEmpty();
        }
    }
}
