using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_AUTENTICACION", Schema = "DBA")]
    public class GENTEMAR_AUTENTICACION
    {
        [Key]
        public long Id { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public string Ip { get; set; }
        public DateTime FechaHoraInicioSesion { get; set; }
        public DateTime? FechaHoraFinSesion { get; set; }
        public int Estado { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Comentario { get; set; }
        public int IdAplicacion { get; set; }
        [NotMapped]
        public bool IsExpired => DateTime.Now >= FechaExpiracion;
    }
}
