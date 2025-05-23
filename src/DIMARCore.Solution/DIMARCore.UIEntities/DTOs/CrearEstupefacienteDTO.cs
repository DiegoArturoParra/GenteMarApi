﻿using DIMARCore.Utilities.Core.ValidAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class CrearEstupefacienteDTO
    {
        public long EstupefacienteId { get; set; }
        public long GenteDeMarId { get; set; }
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números.")]
        [StringLength(19, ErrorMessage = "Longitud mínima de {1} caracteres y máximo de {2} caracteres.", MinimumLength = 12)]
        [Required(ErrorMessage = "Radicado requerido.")]
        public string Radicado { get; set; }
        public int CapitaniaId { get; set; }
        [Required(ErrorMessage = "Estado requerido.")]
        public int EstadoId { get; set; }
        [Required(ErrorMessage = "Tramite requerido.")]
        public int TramiteId { get; set; }
        public EstupefacienteDatosBasicosDTO DatosBasicos { get; set; }
        public DateTime? FechaRadicadoSgdea { get; set; }
    }

    public class EstupefacienteDatosBasicosDTO
    {
        public bool IsExist { get; set; }
        [Required(ErrorMessage = "Nombre requerido.")]
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Identificación requerida.")]
        public string Identificacion { get; set; }
        [DataType(DataType.Date)]
        [MinAge(15)]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "tipo documento requerido.")]
        public int TipoDocumentoId { get; set; }
    }
}



