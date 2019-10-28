using FluentValidation;
using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Helpers.Extensions;
using IndividualsDirectory.Models.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Update
{
    public class UpdateIndividualViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public int? CityId { get; set; }

        public IList<UpdatePhoneNumberViewModel> PhoneNumbers { get; set; }

        public IList<UpdateRelatedIndividualViewModel> RelatedIndividuals { get; set; }

        public string RowVersion { get; set; }

        public static Func<Individual, UpdateIndividualViewModel> Projection
        => (individual) => individual == null ? null : new UpdateIndividualViewModel
        {
            Id = individual.Id,
            FirstName = individual.FirstName,
            LastName = individual.LastName,
            Gender = individual.Gender,
            PersonalNumber = individual.PersonalNumber,
            BirthDate = individual.BirthDate,
            Image = individual.ImageUrl,
            CityId = individual.CityId,
            PhoneNumbers = !individual.PhoneNumbers.Any() 
                ? new List<UpdatePhoneNumberViewModel>() 
                : individual.PhoneNumbers
                    .Select(UpdatePhoneNumberViewModel.Projection)
                    .ToList(),

            RelatedIndividuals = !individual.RelatedIndividuals.Any()
                ? new List<UpdateRelatedIndividualViewModel>()
                : individual.RelatedIndividuals
                    .Select(UpdateRelatedIndividualViewModel.Projection)
                    .ToList(),

            RowVersion = Convert.ToBase64String(individual.RowVersion)
        };

        public static Func<UpdateIndividualViewModel, Individual, Individual> UpdateProjection
        => (updateIndividual, individual) =>
        {
            if (updateIndividual == null || individual == null)
                return null;

            individual.FirstName = updateIndividual.FirstName;
            individual.LastName = updateIndividual.LastName;
            individual.Gender = updateIndividual.Gender;
            individual.PersonalNumber = updateIndividual.PersonalNumber;
            individual.BirthDate = updateIndividual.BirthDate;
            individual.ImageUrl = string.IsNullOrEmpty(updateIndividual.Image) ? individual.ImageUrl : updateIndividual.Image;
            individual.CityId = updateIndividual.CityId;

            var phoneNumbersForDelete = individual.PhoneNumbers
                .Where(i => !updateIndividual.PhoneNumbers
                    .Any(j => j.Id != null && j.Id.Equals(i.Id))).ToList();

            var phoneNumbersForUpdate = updateIndividual.PhoneNumbers
                .Where(i => individual.PhoneNumbers.Any(j => j.Id.Equals(i.Id)));

            var phoneNumbersForAdd = updateIndividual.PhoneNumbers
                .Where(i => i.Id == null);

            foreach (var phoneNumberForDelete in phoneNumbersForDelete)
                individual.PhoneNumbers.Remove(phoneNumberForDelete);

            foreach (var phoneNumberForUpdate in phoneNumbersForUpdate)
                individual.PhoneNumbers
                    .Where(i => i.Id.Equals(phoneNumberForUpdate.Id))
                    .Select(i => UpdatePhoneNumberViewModel.UpdateProjection(phoneNumberForUpdate, i)).ToList();

            foreach (var phoneNumberForAdd in phoneNumbersForAdd)
                individual.PhoneNumbers.Add(CreatePhoneNumberViewModel.Projection(phoneNumberForAdd));

            var relatedIndividualsForDelete = individual.RelatedIndividuals
                .Where(i => !updateIndividual.RelatedIndividuals.Any(j => j.Id != null && j.Id.Equals(i.Id))).ToList();

            var relatedIndividualsForUpdate = updateIndividual.RelatedIndividuals
                .Where(i => individual.RelatedIndividuals.Any(j => j.Id.Equals(i.Id)));

            var relatedIndividualsForAdd = updateIndividual.RelatedIndividuals
                .Where(i => i.Id == null);

            foreach (var relatedIndividualForDelete in relatedIndividualsForDelete)
                individual.RelatedIndividuals.Remove(relatedIndividualForDelete);

            foreach (var relatedIndividualForUpdate in relatedIndividualsForUpdate)
                individual.RelatedIndividuals
                    .Where(i => i.Id.Equals(relatedIndividualForUpdate.Id))
                    .Select(i => UpdateRelatedIndividualViewModel.UpdateProjection(relatedIndividualForUpdate, i)).ToList();

            foreach (var relatedIndividualForAdd in relatedIndividualsForAdd)
                individual.RelatedIndividuals.Add(CreateRelatedIndividualViewModel.Projection(relatedIndividualForAdd));

            return individual;
        };
    }

    public class UpdateIndividualValidator : AbstractValidator<UpdateIndividualViewModel>
    {
        public UpdateIndividualValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty().Length(2, 50).MustBeLatinOrGeoLetters();
            RuleFor(i => i.LastName).NotEmpty().Length(2, 50).MustBeLatinOrGeoLetters();
            RuleFor(i => i.Gender).NotEmpty().IsInEnum();
            RuleFor(i => i.PersonalNumber).NotEmpty().Length(11, 11).OnlyNumeric();
            RuleFor(i => i.BirthDate).NotEmpty().MustBe18YearsOld();
            RuleFor(i => i.CityId).NotEmpty();
            RuleForEach(i => i.PhoneNumbers).SetValidator(new UpdatePhoneNumberValidator());
            RuleForEach(i => i.RelatedIndividuals).SetValidator(new UpdateRelatedIndividualValidator());
        }
    }
}
