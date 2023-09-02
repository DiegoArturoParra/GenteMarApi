namespace GenteMarCore.Entities.Models
{
    using GenteMarCore.Entities.Helpers;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GENTEMAR_DATOSBASICOS", Schema = "DBA")]
    public partial class GENTEMAR_DATOSBASICOS : GENTEMAR_CAMPOS_AUDITORIA
    {
        public GENTEMAR_DATOSBASICOS()
        {

        }

        [Key]
        public long id_gentemar { get; set; }

        [StringLength(16)]
        public string documento_identificacion { get; set; }

        public int id_tipo_documento { get; set; }

        public int? id_municipio_expedicion { get; set; }

        [StringLength(3)]
        public string cod_pais { get; set; }

        public DateTime? fecha_expedicion { get; set; }

        public DateTime? fecha_vencimiento { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public int id_genero { get; set; }

        public DateTime fecha_nacimiento { get; set; }

        public int? id_pais_nacimiento { get; set; }
        public int? id_pais_residencia { get; set; }

        [StringLength(100)]
        public string direccion { get; set; }

        public int id_municipio_residencia { get; set; }

        [StringLength(18)]
        public string telefono { get; set; }

        [StringLength(100)]
        public string correo_electronico { get; set; }

        [StringLength(20)]
        public string numero_movil { get; set; }

        public int id_estado { get; set; }

        public int id_formacion_grado { get; set; }
    }
}
