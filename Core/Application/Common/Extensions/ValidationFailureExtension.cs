using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Extensions
{
    public static class ValidationFailureListExtension
    {
        public static Dictionary<string, string> ToDictionary(this List<ValidationFailure> failures) =>
            failures.GroupJoin(failures,
                               propertyName => propertyName.PropertyName,
                               errorMessage => errorMessage.PropertyName,
                               (propertyName, errorMessages) =>
                               new
                               {
                                   propertyName.PropertyName,
                                   errorMessage = errorMessages.Select(s => s.ErrorMessage)
                               })
                    .DistinctBy(s => s.PropertyName)
                    .ToDictionary(s => $"[{s.PropertyName}]:", s => $"{{{string.Join(',', s.errorMessage)}}}");
    }
}
