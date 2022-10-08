using FluentValidation.Results;
using System.Collections.Generic;

namespace Core.UseCase.Util
{
    internal class FactoryValidationResult
    {
        protected FactoryValidationResult()
        {
        }
        internal static ValidationResult Create(string propertyName, string error) {
            return new ValidationResult(new List<ValidationFailure>() {
                     new ValidationFailure(propertyName,error)
                });
        }
        
    }
}
