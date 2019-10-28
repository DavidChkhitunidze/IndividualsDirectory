using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Get
{
    public class GetPhoneNumberViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public PhoneNumberType? PhoneNumberType { get; set; }

        public static Expression<Func<PhoneNumber, GetPhoneNumberViewModel>> Projection
        => (phoneNumber) => phoneNumber == null ? null : new GetPhoneNumberViewModel
        {
            Id = phoneNumber.Id,
            Number = phoneNumber.Number,
            PhoneNumberType = phoneNumber.PhoneNumberType
        };
    }
}
