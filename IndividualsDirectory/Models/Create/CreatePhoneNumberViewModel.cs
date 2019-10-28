using FluentValidation;
using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Helpers.Extensions;
using IndividualsDirectory.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.Create
{
    public class CreatePhoneNumberViewModel
    {
        public string Number { get; set; }

        public PhoneNumberType? PhoneNumberType { get; set; }

        public static Func<CreatePhoneNumberViewModel, PhoneNumber> Projection
        => createPhoneNumber => createPhoneNumber == null ? null : new PhoneNumber
        {
            Number = createPhoneNumber.Number,
            PhoneNumberType = createPhoneNumber.PhoneNumberType
        };

        public static implicit operator CreatePhoneNumberViewModel(UpdatePhoneNumberViewModel updatePhoneNumber)
        => new CreatePhoneNumberViewModel
        {
            Number = updatePhoneNumber.Number,
            PhoneNumberType = updatePhoneNumber.PhoneNumberType
        };
    }

    public class CreatePhoneNumberValidator : AbstractValidator<CreatePhoneNumberViewModel>
    {
        public CreatePhoneNumberValidator()
        {
            RuleFor(i => i.Number).NotEmpty().OnlyNumeric();
            RuleFor(i => i.PhoneNumberType).NotEmpty().IsInEnum();
        }
    }
}
