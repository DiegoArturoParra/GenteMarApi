using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class ValidFileInBytesTypeAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        public int MaxFileSizeMB { get; set; } // Peso máximo permitido en megabytes
        private int MaxFileSizeBytes => MaxFileSizeMB * 1024 * 1024; // Tamaño máximo en bytes

        public ValidFileInBytesTypeAttribute(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
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

            if (!IsValidFileType(bytes))
            {
                string allowedExtensionsString = string.Join(", ", _allowedExtensions);
                string errorMessage = $"El archivo debe tener una de las siguientes extensiones: {allowedExtensionsString}.";
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        private bool IsValidFileType(byte[] bytes)
        {
            string fileExtension = GetFileExtension(bytes);
            return Array.Exists(_allowedExtensions, extension => string.Equals(extension, fileExtension, StringComparison.OrdinalIgnoreCase));
        }

        private string GetFileExtension(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    if (bytes.Length < 256)
                    {
                        byte[] headerBytes = reader.ReadBytes(bytes.Length);
                        string mimeType = new ContentType().MediaType;
                        string extension = Path.GetExtension(mimeType);
                        return extension;
                    }
                }
            }

            return string.Empty;
        }
    }

}
