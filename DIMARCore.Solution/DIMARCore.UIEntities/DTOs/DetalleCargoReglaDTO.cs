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
        public List<string> Funciones { get; set; }
        public List<string> Habilitaciones { get; set; }
    }
}
