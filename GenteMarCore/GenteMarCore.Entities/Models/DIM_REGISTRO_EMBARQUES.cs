using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GenteMarCore.Entities.Models
{
    [Table("DimRegistroEmbarques", Schema = "DBA")]
    public partial class DIM_REGISTRO_EMBARQUES
    {
        [Key]
        public int idEmbarque { get; set; }
        public string matriculaOMI { get; set; }
        public string nombreNave { get; set; }
        public string cargo { get; set; }
        public string grado { get; set; }
        public decimal trb { get; set; }
        public string potencia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinal { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public decimal idpersona { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string ArchivoBin { get; set; }
        public string FormatoArchivo { get; set; }
    }
}
