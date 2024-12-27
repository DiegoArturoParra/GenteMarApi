using System.ComponentModel.DataAnnotations;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class ValidExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public ValidExtensionAttribute(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string extension)
            {
                extension = extension.ToUpper(); // Convertir la extensión a minúsculas para evitar problemas de mayúsculas/minúsculas

                foreach (var allowedExtension in _allowedExtensions)
                {
                    if (extension.EndsWith(allowedExtension))
                    {
                        return ValidationResult.Success; // La extensión es válida
                    }
                }
            }

            return new ValidationResult($"La extensión no es válida. Las extensiones permitidas son: {string.Join(", ", _allowedExtensions)}");
        }
    }
}
