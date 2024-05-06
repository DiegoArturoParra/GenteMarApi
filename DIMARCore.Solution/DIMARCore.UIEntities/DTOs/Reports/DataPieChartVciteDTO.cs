using DIMARCore.Utilities.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DIMARCore.UIEntities.DTOs.Reports
{
    public class ReportPieChartVciteDTO
    {
        public int CantidadTotal { get; set; }
        public IEnumerable<DataPieChartVciteDTO> DataByChart { get; set; }
        public string Mensaje => this.CantidadTotal > 0 ? $"Total de VCITE en el Gráfico: [{CantidadTotal}]" : string.Empty;
    }

    public class DataPieChartVciteDTO
    {
        [JsonIgnore]
        public string Estado { get; set; }
        [JsonIgnore]
        public string IsVigente { get; set; }
        public int data { get; set; }
        public string label => $"{Estado} - {IsVigente} -> {data}";
        public string color => GenerateRandomColor(Estado, IsVigente);
        static private string GenerateRandomColor(string estado, string isVigente)
        {
            if (estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Negativa)))
            {
                return "#f05050";
            }

            else if (estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Exitosa)) && isVigente.ToUpper().Contains("NO"))
            {
                return "#37bc9b";
            }

            else if (!isVigente.ToUpper().Contains("NO") && estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Exitosa)))
            {
                return "#27c24c";
            }

            else if (estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.ParaEnviar)))
            {
                return "#5d9cec";
            }
            else if (estado.Equals(EnumConfig.GetDescription(EstadoEstupefacienteEnum.Consulta)))
            {
                return "#ff902b";
            }
            else
            {
                return $"#29387b";
            }
        }
    }
}
