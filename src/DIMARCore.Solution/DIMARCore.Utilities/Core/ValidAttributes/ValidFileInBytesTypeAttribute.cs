using System.ComponentModel.DataAnnotations;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class ValidFileInBytesTypeAttribute : ValidationAttribute
    {
        public int MaxFileSizeMB { get; set; } // Peso máximo permitido en megabytes

        private int MaxFileSizeBytes => MaxFileSizeMB * 1024 * 1024; // Tamaño máximo en bytes
        public ValidFileInBytesTypeAttribute()
        {
            MaxFileSizeMB = 5; // Valor predeterminado de 5 MB
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is byte[] bytes))
            {
                return ValidationResult.Success;
            }

            if (bytes.Length > MaxFileSizeBytes)
            {
                string errorMessage = $"El tamaño del archivo supera el límite permitido de {MaxFileSizeMB} MB.";
                return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
