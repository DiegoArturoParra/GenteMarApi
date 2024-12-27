using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("t_nav_med", Schema = "DBA")]
    public partial class DETALLE_NAVE
    {
        [Key]
        public string identi { get; set; }
        public decimal? eslora { get; set; }
        public decimal? manga { get; set; }
        public decimal? puntal { get; set; }
        public decimal? calado { get; set; }
        public decimal? trb { get; set; }
        public decimal? trn { get; set; }
        public decimal? tpm { get; set; }
        public decimal? franco_bordo { get; set; }
        public decimal? cod_andino { get; set; }
        public decimal? tacarreo { get; set; }
    }
}
