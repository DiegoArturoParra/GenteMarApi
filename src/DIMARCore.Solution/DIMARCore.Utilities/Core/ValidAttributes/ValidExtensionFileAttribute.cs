using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class ValidExtensionFileAttribute : ValidationAttribute
    {
        public string[] ValidExtensions { get; set; }

        public ValidExtensionFileAttribute(params string[] validExtensions)
        {
            ValidExtensions = validExtensions.Select(ext => ext.ToUpper()).ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var file = value as HttpPostedFile;
            if (file == null)
                return ValidationResult.Success;

            var ext = Path.GetExtension(file.FileName)?.ToUpper().Replace(".", "");
            if (ext == null || !ValidExtensions.Contains(ext))
            {
                string allowedExtensionsString = string.Join(", ", ValidExtensions);
                string errorMessage = $"El archivo debe tener una de las siguientes extensiones: {allowedExtensionsString}.";
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }

}
