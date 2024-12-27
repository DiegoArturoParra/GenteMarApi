using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.ViewsSQL
{
    [Table("VwGenteMarListarReglasCargo", Schema = "DBA")]
    public class ViewGenteMarListarReglasCargo
    {
        [Key]
        public int CargoReglaId { get; set; }
        public int ReglaId { get; set; }
        public int CargoTituloId { get; set; }
        public int CapacidadId { get; set; }
        public int SeccionId { get; set; }
        public int NivelId { get; set; }
        public string Regla { get; set; }
        public string CargoTitulo { get; set; }
        public string Nivel { get; set; }
        public string Capacidad { get; set; }
        public string Seccion { get; set; }
        public string Funciones { get; set; }
        public string Habilitaciones { get; set; }
    }
}
