using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Get
{
    public class GetCityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<City, GetCityViewModel>> Projection
        => city => city == null ? null : new GetCityViewModel
        {
            Id = city.Id,
            Name = city.Name
        };
    }
}
