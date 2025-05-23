﻿using GenteMarCore.Entities.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_ANTECEDENTES_DATOSBASICOS", Schema = "DBA")]
    public partial class GENTEMAR_ANTECEDENTES_DATOSBASICOS : GENTEMAR_CAMPOS_AUDITORIA
    {
        [Key]
        public long id_gentemar_antecedente { get; set; }
        public byte id_tipo_documento { get; set; }
        public string identificacion { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public DateTime fecha_nacimiento { get; set; }
    }
}
