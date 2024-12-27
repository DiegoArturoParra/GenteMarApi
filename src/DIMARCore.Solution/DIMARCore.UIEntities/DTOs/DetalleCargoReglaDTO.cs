using DIMARCore.Utilities.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs
{
    public class ListadoDetalleCargoReglaDTO
    {
        public string CargoTitulo { get; set; }
        public string Seccion { get; set; }
        public int CargoReglaId { get; set; }
        public string Regla { get; set; }
        public string Nivel { get; set; }
        public string Capacidad { get; set; }
        [JsonIgnore]
        public string FuncionesString { get; set; }
        [JsonIgnore]
        public string HabilitacionesString { get; set; }
        public List<string> Funciones => Reutilizables.GetDelimitedList(FuncionesString, ',');
        public List<string> Habilitaciones => Reutilizables.GetDelimitedList(HabilitacionesString, ',');
    }
}
