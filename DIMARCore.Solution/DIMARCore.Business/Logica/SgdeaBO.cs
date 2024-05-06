using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.QueryFilters;
using GenteMarCore.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class SgdeaBO
    {
        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosTitulosByCedula(string cedula)
        {
            return await new SGDEARepository().GetRadicadosTitulosByCedula(cedula);
        }

        public async Task<IEnumerable<RadicadoDTO>> GetRadicadosLicenciasByCedula(string cedula)
        {
            return await new SGDEARepository().GetRadicadosLicenciaByCedula(cedula);
        }

        public async Task<IEnumerable<RadicadoInfoDTO>> GetRadicadosInfoPersonaParaEstupefacientes(RadicadoSGDEAFilter filter, CancellationToken cancellationToken)
        {
            using (var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var data = await new SGDEARepository().GetRadicadosInfoPersonaParaEstupefacientes(filter, tokenSource);
                return data;
            }
        }
        /// <summary>
        /// Obtiene las previstas por el numero de rtadicado el estado y tipo de tramite 
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<SGDEA_PREVISTAS> GetPrevistaEstado(decimal radicado, string estado, string tramite)
        {
            return await new SGDEARepository().GetWithConditionAsync(x => x.radicado == radicado
            && x.tipo_tramite.Contains(tramite) && x.estado == estado);
        }
    }
}
