using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Get
{
    public class GetIndividualPreviewViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ImageUrl { get; set; }

        public static Expression<Func<Individual, GetIndividualPreviewViewModel>> Projection
        => (person) => person == null ? null : new GetIndividualPreviewViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = person.Gender,
            PersonalNumber = person.PersonalNumber,
            BirthDate = person.BirthDate,
            ImageUrl = person.ImageUrl
        };
    }
}
