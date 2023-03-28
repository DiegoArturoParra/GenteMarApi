using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("DimPersonas", Schema = "DBA")]
    public partial class DIM_PERSONAS
    {
        [Key]
        public decimal idpersona { get; set; }
        public int? idusuario { get; set; }
        public string cedula { get; set; }
        public string firmanueva { get; set; }
        public string huellanueva { get; set; }
        public DateTime? fechacaptura { get; set; }
        public bool impreso { get; set; }
        public DateTime? fechaimpreso { get; set; }
        public bool nuevo { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int? etiqueta { get; set; }
        public int? libreta { get; set; }
        public DateTime? fechaexpiracion { get; set; }
        public string huella { get; set; }
        public int? dedo { get; set; }
        public string FirmaBin { get; set; }
        public string FormatoFirma { get; set; }
        public string FotoBin { get; set; }
        public string FormatoFoto { get; set; }
        public DateTime? FechaEdicionCaptura { get; set; }
    }
}
