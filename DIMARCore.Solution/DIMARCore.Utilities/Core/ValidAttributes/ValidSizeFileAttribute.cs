using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DIMARCore.Utilities.Core.ValidAttributes
{
    public class ValidSizeFileAttribute : ValidationAttribute
    {
        public int MaxFileSizeMB { get; set; } // Peso máximo permitido en megabytes
        private int MaxFileSizeBytes => MaxFileSizeMB * 1024 * 1024; // Tamaño máximo en bytes

        public ValidSizeFileAttribute()
        {
            MaxFileSizeMB = 5;
            ErrorMessage = $"El tamaño del archivo supera el límite permitido de {MaxFileSizeMB} MB.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var file = (HttpPostedFile)value;

            if (file.ContentLength > MaxFileSizeBytes)
            {
                ErrorMessage = $"El tamaño del archivo supera el límite permitido de {MaxFileSizeMB} MB.";
                return false;
            }
            return true;
        }
    }
}
