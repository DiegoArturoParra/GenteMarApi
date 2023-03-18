namespace GenteMarCore.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class GENTEMAR_LICENCIAS
    {
        [NotMapped]
        public GENTEMAR_OBSERVACIONES_LICENCIAS Observacion { get; set; }

    }
}
