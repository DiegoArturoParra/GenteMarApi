using DIMARCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIMARCore.UIEntities.QueryFilters
{
    public class EstupefacientesFilter
    {
        /// <summary>
        /// parametros de la paginación
        /// </summary>
        public ParametrosPaginacion Paginacion { get; set; }
        [StringLength(22, ErrorMessage = "Debe tener una longitud mínima de {2} y una longitud máxima de {1}.", MinimumLength = 4)]
        public string Identificacion { get; set; }
        public string Radicado { get; set; }
        public List<int> EstadosId { get; set; }
        public int TramiteId { get; set; }
        public int ConsolidadoId { get; set; }
        public int ExpedienteId { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int TipoDocumentoId { get; set; }
    }
}
