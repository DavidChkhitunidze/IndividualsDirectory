using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Get
{
    public class GetIndividualViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ImageUrl { get; set; }

        public string City { get; set; }

        public IQueryable<GetPhoneNumberViewModel> PhoneNumbers { get; set; }

        public IQueryable<GetRelatedIndividualViewModel> RelatedIndividuals { get; set; }

        public static Expression<Func<Individual, GetIndividualViewModel>> Projection
        => (person) => person == null ? null : new GetIndividualViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = person.Gender,
            PersonalNumber = person.PersonalNumber,
            BirthDate = person.BirthDate,
            ImageUrl = person.ImageUrl,
            City = person.City == null ? null : person.City.Name,
            PhoneNumbers = person.PhoneNumbers.AsQueryable().Select(GetPhoneNumberViewModel.Projection),
            RelatedIndividuals = person.RelatedIndividuals.AsQueryable().Select(GetRelatedIndividualViewModel.Projection)
        };
    }
}
