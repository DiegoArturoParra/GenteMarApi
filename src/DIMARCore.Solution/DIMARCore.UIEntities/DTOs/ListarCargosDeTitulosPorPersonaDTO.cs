using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListarCargosDeTitulosPorPersonaDTO
    {
        public long TituloCargoReglaId { get; set; }
        public string Capacidad { get; set; }
        public string Regla { get; set; }
        public string Seccion { get; set; }
        public int ReglaCargoId { get; set; }
        public string CargoTitulo { get; set; }
        public string Nivel { get; set; }
        public int NivelId { get; set; }
        public int CargoId { get; set; }
        public int SeccionId { get; set; }
        public int ReglaId { get; set; }
        public int CapacidadId { get; set; }
        public string HabilitacionesJson { get; set; }
        public string FuncionesJson { get; set; }
        public List<HabilitacionCargoDTO> Habilitaciones { get; set; }
        public List<FuncionCargoDTO> Funciones { get; set; }
    }
}
