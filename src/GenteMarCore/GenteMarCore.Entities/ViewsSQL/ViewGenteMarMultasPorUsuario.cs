using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarMultasPorUsuario", Schema = "DBA")]
    public class ViewGenteMarMultasPorUsuario
    {
        [Key]
        public string NumDocumento { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoMulta { get; set; }
        public string EstadoFinal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
