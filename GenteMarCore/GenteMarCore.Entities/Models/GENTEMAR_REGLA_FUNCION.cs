namespace GenteMarCore.Entities.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_REGLA_FUNCION", Schema = "DBA")]
    public partial class GENTEMAR_REGLA_FUNCION
    {
        public int id_regla { get; set; }

        public int id_funcion { get; set; }

        public virtual GENTEMAR_FUNCIONES GENTEMAR_FUNCIONES { get; set; }
        public virtual GENTEMAR_REGLAS GENTEMAR_REGLAS { get; set; }
    }
}
