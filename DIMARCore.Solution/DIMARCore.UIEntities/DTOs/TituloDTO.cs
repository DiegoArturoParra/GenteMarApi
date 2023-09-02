using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.DTOs
{
    public class TituloDTO
    {
        public long TituloId { get; set; }
        public List<CargosReglaDTO> CargosDelTitulo { get; set; }
        public long GenteMarId { get; set; }
        [Required(ErrorMessage = "Fecha vencimiento requerida.")]
        public string FechaVencimiento { get; set; }
        [Required(ErrorMessage = "Fecha expedición requerida.")]
        public string FechaExpedicion { get; set; }
        [StringLength(3, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 3)]
        [Required(ErrorMessage = "id pais requerido.")]
        public string CodigoPais { get; set; }
        [Required(ErrorMessage = "id tramite requerido.")]
        public int EstadoTramiteId { get; set; }
        [Required(ErrorMessage = "id capitania requerido.")]
        public int CapitaniaId { get; set; }
        [Required(ErrorMessage = "id solicitud requerido.")]
        public int TipoSolicitudId { get; set; }
        [RegularExpression("(^[0-9.]+$)", ErrorMessage = "Solo se permiten números y punto decimal.")]
        [StringLength(19, ErrorMessage = "Longitud mínima de {1} caracteres y máximo de {2} caracteres.", MinimumLength = 12)]
        [Required(ErrorMessage = "Radicado requerido.")]
        public string Radicado { get; set; }
        [Required(ErrorMessage = "id capitania firmante requerido.")]
        public int CapitaniaFirmanteId { get; set; }
        public ObservacionDTO Observacion { get; set; }
        [Required(ErrorMessage = "id tipo refrendo requerido.")]
        public int TipoRefrendoId { get; set; }

    }

    public class CargosReglaDTO
    {
        [Required(ErrorMessage = "id tablas requeridos requeridas.")]
        public IdsTablasForaneasDTO IdsLlaveCompuesta { get; set; }
        public int CargoReglaId { get; set; }
        public long TituloCargoReglaId { get; set; }
        public List<int> HabilitacionesId { get; set; }
        [Required(ErrorMessage = "Se requiere por lo menos una función.")]
        public List<int> FuncionesId { get; set; }
    }
}
