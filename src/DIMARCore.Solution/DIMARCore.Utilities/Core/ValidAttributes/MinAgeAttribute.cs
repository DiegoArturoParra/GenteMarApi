using System;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var birthDate = (DateTime)value;

            if (birthDate > DateTime.Today.AddYears(-_minAge))
            {
                return new ValidationResult($"La persona debe tener al menos {_minAge} años de edad.");
            }
            return ValidationResult.Success;
        }
    }
}
