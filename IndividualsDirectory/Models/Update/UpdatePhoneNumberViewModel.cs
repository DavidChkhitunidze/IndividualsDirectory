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
    public class UpdatePhoneNumberViewModel
    {
        public int? Id { get; set; }

        public string Number { get; set; }

        public PhoneNumberType? PhoneNumberType { get; set; }

        public static Func<PhoneNumber, UpdatePhoneNumberViewModel> Projection
        => (phoneNumber) => phoneNumber == null ? null : new UpdatePhoneNumberViewModel
        {
            Id = phoneNumber.Id,
            Number = phoneNumber.Number,
            PhoneNumberType = phoneNumber.PhoneNumberType
        };

        public static Func<UpdatePhoneNumberViewModel, PhoneNumber, PhoneNumber> UpdateProjection
        => (updatePhoneNumber, phoneNumber) =>
        {
            if (updatePhoneNumber == null || phoneNumber == null)
                return null;

            phoneNumber.Number = updatePhoneNumber.Number;
            phoneNumber.PhoneNumberType = updatePhoneNumber.PhoneNumberType;

            return phoneNumber;
        };
    }

    public class UpdatePhoneNumberValidator : AbstractValidator<UpdatePhoneNumberViewModel>
    {
        public UpdatePhoneNumberValidator()
        {
            RuleFor(i => i.Number).NotEmpty().OnlyNumeric();
            RuleFor(i => i.PhoneNumberType).NotEmpty().IsInEnum();
        }
    }
}
