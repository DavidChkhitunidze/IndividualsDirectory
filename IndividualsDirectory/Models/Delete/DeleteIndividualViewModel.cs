using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Delete
{
    public class DeleteIndividualViewModel
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public static Func<Individual, DeleteIndividualViewModel> Projection
        => individual => individual == null ? null : new DeleteIndividualViewModel
        {
            Id = individual.Id,
            Image = individual.ImageUrl
        };
    }
}
