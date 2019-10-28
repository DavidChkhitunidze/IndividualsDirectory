using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IndividualsDirectory.Helpers.Extensions
{
    public static class CustomFluentValidators
    {
        public static IRuleBuilderOptions<T, DateTime?> MustBe18YearsOld<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
            => ruleBuilder.Must((rootObj, dateTime, context) =>
            {
                var dateTimeNow = DateTime.Now;
                var yearsAgo = new DateTime(dateTimeNow.Year - 18, dateTimeNow.Month, dateTimeNow.Day);

                return dateTime < yearsAgo;
            }).WithMessage("Must be 18 years old.");

        public static IRuleBuilderOptions<T, string> MustBeLatinOrGeoLetters<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Must((rootObj, input, context) =>
            {
                var onlyLatin = Regex.IsMatch(input, @"^[a-zA-Z]+$");
                var onlyGeo = Regex.IsMatch(input, @"^[ა-ჰ]+$");

                return onlyLatin || onlyGeo;
            }).WithMessage("Should contain only Latin or Georgian letters.");

        public static IRuleBuilderOptions<T, string> OnlyNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Must((rootObj, input, context) =>
            {
                return Regex.IsMatch(input, @"^[0-9]+$");
            }).WithMessage("Should contain only numeric values.");
    }
}
