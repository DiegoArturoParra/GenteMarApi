using System;

namespace DIMARCore.UIEntities.DTOs
{
    public class DimRegistroEmbarqueDTO
    {
        public int idEmbarque { get; set; }
        public string MatriculaOMI { get; set; }
        public string NombreNave { get; set; }
        public string Cargo { get; set; }
        public string Grado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal DifDias { get; set; }
        public decimal TotMes { get; set; }
        public decimal trb { get; set; }
        public string potencia { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public decimal idpersona { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string ArchivoBin { get; set; }
        public string FormatoArchivo { get; set; }
    }
}
