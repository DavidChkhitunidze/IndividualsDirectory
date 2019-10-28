using FluentValidation;
using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Create
{
    public class CreateIndividualViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public int? CityId { get; set; }

        public IList<CreatePhoneNumberViewModel> PhoneNumbers { get; set; }

        public IList<CreateRelatedIndividualViewModel> RelatedIndividuals { get; set; }

        public static Func<CreateIndividualViewModel, Individual> Projection
        => createIndividual => createIndividual == null ? null : new Individual
        {
            FirstName = createIndividual.FirstName,
            LastName = createIndividual.LastName,
            Gender = createIndividual.Gender,
            PersonalNumber = createIndividual.PersonalNumber,
            BirthDate = createIndividual.BirthDate,
            ImageUrl = string.IsNullOrEmpty(createIndividual.Image) ? "no-avatar.png" : createIndividual.Image,
            CityId = createIndividual.CityId,
            PhoneNumbers = createIndividual.PhoneNumbers == null 
                ? new List<PhoneNumber>()
                : createIndividual.PhoneNumbers
                    .Where(i => !string.IsNullOrEmpty(i.Number) && i.PhoneNumberType != null)
                    .Select(CreatePhoneNumberViewModel.Projection)
                    .ToList(),

            RelatedIndividuals = createIndividual.RelatedIndividuals == null 
                ? new List<RelatedIndividual>() 
                : createIndividual.RelatedIndividuals.Select(CreateRelatedIndividualViewModel.Projection).ToList()
        };
    }

    public class CreateIndividualValidator : AbstractValidator<CreateIndividualViewModel>
    {
        public CreateIndividualValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty().Length(2, 50).MustBeLatinOrGeoLetters();
            RuleFor(i => i.LastName).NotEmpty().Length(2, 50).MustBeLatinOrGeoLetters();
            RuleFor(i => i.Gender).NotEmpty().IsInEnum();
            RuleFor(i => i.PersonalNumber).NotEmpty().Length(11, 11).OnlyNumeric();
            RuleFor(i => i.BirthDate).NotEmpty().MustBe18YearsOld();
            RuleFor(i => i.CityId).NotEmpty();
            RuleForEach(i => i.PhoneNumbers).SetValidator(new CreatePhoneNumberValidator());
            RuleForEach(i => i.RelatedIndividuals).SetValidator(new CreateRelatedIndividualValidator());
        }
    }
}
